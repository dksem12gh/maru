using UnityEngine;
using UnityEngine.Events;


namespace DigitalMaru.MiniGame.MusicMovement
{
    public class Miss_event : MonoBehaviour
    {
        NoteGenerator noteGenerator;
        JudgementManager judgementManager;

        public void SetNoteGenerator(NoteGenerator _noteGenerator, JudgementManager _judgementManager)
        {
            noteGenerator = _noteGenerator;
            judgementManager = _judgementManager;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Note"))
            {
                other.transform.GetComponent<Note>().TouchEnable(false);
                judgementManager.MissEvent();
                noteGenerator.ReturnToPool(other.transform.GetComponent<Note>());
            }
        }
    }
}