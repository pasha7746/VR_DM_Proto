using UnityEngine;
using UnityEngine.UI;
using System.Data;
using System;
using Mono.Data.SqliteClient;
using System.Collections.Generic;
//using Mono.Data.Sqlite;
using System.IO;

public class MainAdd : MonoBehaviour
{
    public GameObject img;

    public InputField nl;
    public InputField nf;

    public RawImage rmimg;

    public IDbConnection dbconn;
    public IDbCommand dbcmd;
    public IDataReader reader;

    public Text txt;
    public Text dtxt;

    [HideInInspector]
    public int pid;
    [HideInInspector]
    public int relativechosen;

    [HideInInspector]
    public string conn;

    [HideInInspector]
    public string path = "";

    void Start ()
    {
        if (Application.platform != RuntimePlatform.Android)
        {
            path = Application.dataPath + "/StreamingAssets/PeopleDB.db";
        }
        else
        {
            path = Application.persistentDataPath + "/PeopleDB.db";
        }
        OpenDB("PeopleDB.db");
        pid = GetPersonPID() + 1;
    }
    public string PopulatePerson()
    {
        string conn = "";
        string res = "";
        conn = "URI=file:" + path;

        dbconn = (IDbConnection)new SqliteConnection(conn);
        dbconn.Open();

        dbcmd = dbconn.CreateCommand();

        string sqlQuery = "SELECT FirstName,Lastname FROM persons where PID = " + pid;
        dbcmd.CommandText = sqlQuery;
        reader = dbcmd.ExecuteReader();

        reader.Read();

        res = reader.GetString(1) + "," + reader.GetString(0);

        reader.Close();
        reader = null;
        dbcmd.Dispose();
        dbcmd = null;
        dbconn.Close();
        dbconn = null;

        return res; // return matches
    }
    void LoadImage()
    {
        string conn = "";

        conn = "URI=file:" + path;

        dbconn = (IDbConnection)new SqliteConnection(conn);
        dbconn.Open();

        dbcmd = dbconn.CreateCommand();

        string query = "SELECT Photo FROM persons WHERE PID=" + pid + ";";

        dbcmd.CommandText = query;
        //reader = dbcmd.ExecuteReader();
        byte[] data = (byte[])dbcmd.ExecuteScalar();

        if (data != null)
        {
            //File.WriteAllBytes("woman2.jpg", data);
            Texture2D sampleTexture = new Texture2D(2, 2);
            bool isLoaded = sampleTexture.LoadImage(data);
            if (isLoaded)
            {
                img.GetComponent<RawImage>().texture = sampleTexture;
                rmimg.texture = sampleTexture;
            }
        }
        reader.Close();
        reader = null;
        dbcmd.Dispose();
        dbcmd = null;
        dbconn.Close();
        dbconn = null;
    }
    public void ButtonUpdateOnClick()
    {
        string ln = "";
        string fn = "";
        string pth = "";

        ln = nl.text;
        pth = dtxt.text;
        fn = nf.text;

        WorkPID();

        if (pth == "")
        {
            PersonData(ln, fn);
        }
        else
        {
            PersonPhotoData(fn, ln, pth);
        }
        VacuumDB();
    }
    public void PersonData(string ln1, string fn1)
    {
        string conn = "";
        conn = "URI=file:" + path;

        dbconn = (IDbConnection)new SqliteConnection(conn);
        dbconn.Open();

        IDbCommand dbcmd = dbconn.CreateCommand();

        string sqlQuery = "INSERT INTO persons (PID,FirstName,LastName) VALUES (" + pid + ",'" + fn1 + "','" + ln1 + "')";

        dbcmd.CommandText = sqlQuery;
        dbcmd.ExecuteNonQuery();

        dbcmd.Dispose();
        dbcmd = null;
        dbconn.Close();
        dbconn = null;
    }
    public void PersonPhotoData(string ln1, string fn1, string pth)
    {
        string conn = "";
        conn = "URI=file:" + path;

        dbconn = (IDbConnection)new SqliteConnection(conn);
        dbconn.Open();

        byte[] data = null;

        data = File.ReadAllBytes(pth);

        IDbCommand dbcmd = dbconn.CreateCommand();

        //       dbcmd.CommandText = "UPDATE persons SET Photo = @img where PID = " + tid;
        dbcmd.CommandText = "INSERT INTO persons (Photo,PID,FirstName,LastName) VALUES (@img," + pid + ",'" + fn1 + "','" + ln1 + "');";
        dbcmd.Prepare();

        SqliteParameter param = new SqliteParameter("@img", DbType.Binary, data.Length);
        param.Value = data;
        dbcmd.Parameters.Add(param);

        dbcmd.ExecuteNonQuery();

        dbcmd.Dispose();
        dbcmd = null;
        dbconn.Close();
        dbconn = null;
    }
    public int GetPID()
    {
        string conn = "";
        int res = 0;
        conn = "URI=file:" + path;

        dbconn = (IDbConnection)new SqliteConnection(conn);
        dbconn.Open();

        dbcmd = dbconn.CreateCommand();

        string sqlQuery = "SELECT PID FROM working";
        dbcmd.CommandText = sqlQuery;
        reader = dbcmd.ExecuteReader();

        reader.Read();
        
        res = reader.GetInt32(0);
        
        reader.Close();
        reader = null;
        dbcmd.Dispose();
        dbcmd = null;
        dbconn.Close();
        dbconn = null;

        return res; // return matches
    }
    public int GetPersonPID()
    {
        string conn = "";
        int res = 0;
        conn = "URI=file:" + path;

        dbconn = (IDbConnection)new SqliteConnection(conn);
        dbconn.Open();

        dbcmd = dbconn.CreateCommand();

        string sqlQuery = "SELECT count(*) FROM persons";
        dbcmd.CommandText = sqlQuery;
        reader = dbcmd.ExecuteReader();

        reader.Read();

        res = reader.GetInt32(0);

        reader.Close();
        reader = null;
        dbcmd.Dispose();
        dbcmd = null;
        dbconn.Close();
        dbconn = null;

        return res; // return matches
    }
    public void OpenDB(string p)
    {
        if (!File.Exists(path))
        {
            WWW loadDB = new WWW("jar:file://" + Application.dataPath + "!/assets/" + p);
            while (!loadDB.isDone) { }
            // then save to Application.persistentDataPath
            File.WriteAllBytes(path, loadDB.bytes);
        }
    }
    public void WorkPID()
    {
        string conn = "";

        conn = "URI=file:" + path;


        dbconn = (IDbConnection)new SqliteConnection(conn);
        dbconn.Open();

        IDbCommand dbcmd = dbconn.CreateCommand();

        string sqlQuery = "update working set RID = 0, PID = " + pid;
        dbcmd.CommandText = sqlQuery;
        dbcmd.ExecuteNonQuery();
        dbcmd.Dispose();
        dbcmd = null;
        dbconn.Close();
        dbconn = null;
    }
    public void VacuumDB()
    {
        string conn = "";
        conn = "URI=file:" + path;


        dbconn = (IDbConnection)new SqliteConnection(conn);
        dbconn.Open();

        IDbCommand dbcmd = dbconn.CreateCommand();

        string sqlQuery = "PRAGMA auto_vacuum = FULL";
        dbcmd.CommandText = sqlQuery;
        dbcmd.ExecuteNonQuery();
        dbcmd.Dispose();
        dbcmd = null;
        dbconn.Close();
        dbconn = null;
    }
}
