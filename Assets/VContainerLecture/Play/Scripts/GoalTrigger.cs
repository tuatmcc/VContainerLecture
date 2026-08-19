using UnityEngine;
using VContainer;

namespace VContainerLecture.Play.Scripts
{
    public class GoalTrigger : MonoBehaviour
    {
        private IPlayManager _playManager;

        [Inject]
        public void Construct(IPlayManager playManager)
        {
            _playManager = playManager;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player"))
            {
                return;
            }
            
            
        }
    }
}