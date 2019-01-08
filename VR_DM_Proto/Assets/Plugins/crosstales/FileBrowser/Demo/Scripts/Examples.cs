using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace Crosstales.FB.Demo
{
    public class Examples : MonoBehaviour
    {
        public GameObject img;
        public Text txt;
        public GameObject TextPrefab;
        public GameObject ScrollView;
        public Text Error;

        IEnumerator LoadImage()
        {
            string path = OpenSingleFile();
            txt.text = path;
            WWW www = new WWW(path);
            while (!www.isDone)
                yield return null;
            img.GetComponent<RawImage>().texture = www.texture;
        }
        public void AddImage()
        {
            StartCoroutine(LoadImage());
        }
        public string OpenSingleFile()
        {
            
            string extensions = string.Empty;
            
            string path = FileBrowser.OpenSingleFile("Open File", string.Empty, extensions);

            return path;
        }
        public void OpenFiles()
        {
          
            string extensions = string.Empty;
            
            string[] paths = FileBrowser.OpenFiles("Open Files", string.Empty, extensions, true);

            rebuildList(paths);
        }
        public void OpenSingleFolder()
        {
            
            string path = FileBrowser.OpenSingleFolder("Open Folder");

            rebuildList(path);
        }
        public void OpenFolders()
        {
            string[] paths = FileBrowser.OpenFolders("Open Folders");
            
            rebuildList(paths);
        }
        public void SaveFile()
        {
           
            string extensions = "txt";
            
            string path = FileBrowser.SaveFile("Save File", string.Empty, "MySaveFile", extensions);
            
            rebuildList(path);
        }
        public void OpenFilesAsync()
        {
            
            //string extensions = string.Empty;
            
            //FileBrowser.OpenFilesAsync("Open Files", string.Empty, extensions, true, (string[] paths) => { writePaths(paths); });
        }
        public void OpenFoldersAsync()
        {
            
            //FileBrowser.OpenFoldersAsync("Open Folders", string.Empty, true, (string[] paths) => { writePaths(paths); });
        }
        public void SaveFileAsync()
        {
            
            //string extensions = "txt";
            
            //FileBrowser.SaveFileAsync("Save File", string.Empty, "MySaveFile", extensions, (string paths) => { writePaths(paths); });
        }
        private void writePaths(params string[] paths)
        {
            
            rebuildList(paths);
        }
        private void rebuildList(params string[] e)
        {
            for (int ii = ScrollView.transform.childCount - 1; ii >= 0; ii--)
            {
                Transform child = ScrollView.transform.GetChild(ii);
                child.SetParent(null);
                Destroy(child.gameObject);
            }

            ScrollView.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 80 * e.Length);

            for (int ii = 0; ii < e.Length; ii++)
            {

                GameObject go = Instantiate(TextPrefab);

                go.transform.SetParent(ScrollView.transform);
                go.transform.localScale = Vector3.one;
                go.transform.localPosition = new Vector3(10, -80 * ii, 0);
                go.GetComponent<Text>().text = e[ii].ToString();
            }
        }
    }
}
