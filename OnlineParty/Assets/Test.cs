using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Database;

public class Test : MonoBehaviour
{
    DatabaseReference reference;
    // Start is called before the first frame update

    void UpdateScore()
    {
        FirebaseDatabase.DefaultInstance.GetReference("counter").GetValueAsync().ContinueWith(task =>
        {
            if (task.IsFaulted)
            {
                print(task);
                return;
            }

            if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                int value = int.Parse(snapshot.Value.ToString());
                value++;
                reference.Child("counter").SetValueAsync(value);
            }
        });
    }

    // Update is called once per frame
    void Update()
    {
    }
}           
