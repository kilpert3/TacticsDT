using UnityEngine;
using UnityEditor;

public class YourClassAsset
{
    //uses scriptableObjectUtility to create assets. 


    //[in this case, create conversation assets]
    [MenuItem("Assets/Create/Conversation Data")]
    public static void CreateConversationData()
    {
        ScriptableObjectUtility.CreateAsset<ConversationData>();
    }
}