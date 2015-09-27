using UnityEngine;
using UnityEditor;
using System.Xml;
using System.IO;

public class CreatePrefabXML
{
    [MenuItem("sxh/根据prefab生成xml文件")]
    public static void DoIt()
    {
        UnityEngine.Object[] objects = Selection.objects;
        foreach (UnityEngine.Object obj in objects)
        {
            string path = AssetDatabase.GetAssetPath(obj);
            GameObject go = AssetDatabase.LoadAssetAtPath<GameObject>(path);
            createXML(go);
        }
    }

    public static string PATH = Application.dataPath + "/PlayGround/dataTest.xml";
    private static void buildHead(XmlDocument xmlDoc)
    {
        if (File.Exists(PATH))
            File.Delete(PATH);
        XmlDeclaration xmldecl = xmlDoc.CreateXmlDeclaration("1.0", "UTF-8", null);
        xmlDoc.AppendChild(xmldecl);
    }

    private static void createXML(GameObject go)
    {
        XmlDocument xmlDoc = new XmlDocument();
        buildHead(xmlDoc);
        buildBodyByGo(xmlDoc, go);
        xmlDoc.Save(PATH);     
    }

    private static void buildBodyByGo(XmlDocument xmlDoc,GameObject go)
    {
        XmlElement root = xmlDoc.CreateElement("", "root", "");
        xmlDoc.AppendChild(root);
        resursionCreateNode(xmlDoc,root,go);
    }

    private static void resursionCreateNode(XmlDocument xmlDoc,XmlElement parentEle, GameObject go)
    {
        XmlElement goEle = xmlDoc.CreateElement(go.name);
        goEle.SetAttribute("name",go.name);
        parentEle.AppendChild(goEle);

        if (go.transform.childCount == 0)
            return;
        for (int i = 0; i < go.transform.childCount;++i )
        {
            GameObject temp = go.transform.GetChild(i).gameObject;
            resursionCreateNode(xmlDoc, goEle, temp);
        }
    }
}