using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Auth;
using Firebase.Storage;
using Firebase.Firestore;
using Firebase.Extensions;

public class FirebaseDataManager : MonoBehaviour
{
    //FirebaseFirestore db;
    public TeacherData teacherData;

    void Start()
    {
        FirebaseAuthManager.Instance.LoginState += OnChangedState;
    }
    async void OnChangedState(bool sign)
    {
        /*outputText.text = sign ? "로그인 :" : "로그아웃 :";
        outputText.text += FirebaseAuthManager.Instance.UserId;*/
        if (!sign) return;

        FirebaseFirestore db = FirebaseFirestore.DefaultInstance;
        CollectionReference student = db.Collection("Student");
        CollectionReference teacher = db.Collection("Teacher");

        QuerySnapshot studentSnap = await student.GetSnapshotAsync();
        QuerySnapshot teacherSnap = await teacher.GetSnapshotAsync();               

        teacherData._id = FirebaseAuthManager.Instance.UserId;

        foreach (DocumentSnapshot doc in teacherSnap.Documents)
        {
            Dictionary<string, object> documentDictionary = doc.ToDictionary();            
            teacherData._name = documentDictionary["Name"] as string;            
            teacherData._schoolName = documentDictionary["SchoolName"] as string;
            //teacherData.Grade = documentDictionary["Grade"] as ;
            //teacherData.Class = documentDictionary["Class"];
        }

        /*DocumentReference tc = db.Collection("Teacher").Document(FirebaseAuthManager.Instance.UserId);

        tc.GetSnapshotAsync().ContinueWithOnMainThread(task =>
        {

        })*/

    }
}
