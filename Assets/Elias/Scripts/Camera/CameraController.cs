using Cinemachine;
using UnityEngine;
using Elias.Scripts.Managers;

namespace Elias.Scripts.Camera
{
    public class CameraController : MonoBehaviour
    {
        public CinemachineTargetGroup TargetGroup;
        public float moveSpeed = 2f;
        private GameObject[] dummyTargets;
        public Canvas backgroundCanvas;
        private float backgroundMultiplier = -100f; // Negative value to invert the movement
        private Vector3 initialBackgroundPosition;
        private Vector3 initialTargetGroupPosition;
        private Vector3 targetBackgroundPosition;

        private void Start()
        {
            if (TargetGroup == null)
            {
                Debug.LogError("TargetGroup reference is not assigned!");
            }
            GameManager.Instance.OnBlocListUpdated += UpdateTargetGroup;
            dummyTargets = new GameObject[3];
            for (int i = 0; i < dummyTargets.Length; i++)
            {
                dummyTargets[i] = new GameObject("DummyTarget_" + i);
                dummyTargets[i].transform.position = new Vector3(0f, 0f, 0f);
            }

            // Store the initial positions
            initialBackgroundPosition = backgroundCanvas.transform.position;
            initialTargetGroupPosition = TargetGroup.transform.position;
            targetBackgroundPosition = initialBackgroundPosition;
        }

        private void OnDestroy()
        {
            GameManager.Instance.OnBlocListUpdated -= UpdateTargetGroup;
            foreach (var dummy in dummyTargets)
            {
                if (dummy != null)
                {
                    Destroy(dummy);
                }
            }
        }

        private void Update()
        {
            UpdateBackgroundPosition();
            SmoothBackgroundTransition();
        }

        private void UpdateTargetGroup()
        {
            ClearTargetGroup();

            var blocs = GameManager.Instance.Blocs.FindAll(bloc => bloc != null);

            blocs.Sort((a, b) => b.transform.position.y.CompareTo(a.transform.position.y));

            int count = Mathf.Min(3, blocs.Count);
            for (int i = 0; i < count; i++)
            {
                var bloc = blocs[i];
                if (bloc != null && bloc.CompareTag("Solid"))
                {
                    dummyTargets[i].transform.position = new Vector3(0f, bloc.transform.position.y, 0f);
                    TargetGroup.AddMember(dummyTargets[i].transform, 1f, 0f);
                }
            }
        }

        private void ClearTargetGroup()
        {
            while (TargetGroup.m_Targets.Length > 0)
            {
                TargetGroup.RemoveMember(TargetGroup.m_Targets[0].target);
            }
        }

        private void UpdateBackgroundPosition()
        {
            if (TargetGroup != null && backgroundCanvas != null)
            {
                Vector3 currentTargetGroupPosition = TargetGroup.transform.position;
                float targetGroupYOffset = currentTargetGroupPosition.y - initialTargetGroupPosition.y;
                targetBackgroundPosition = initialBackgroundPosition + new Vector3(0f, targetGroupYOffset * backgroundMultiplier, 0f);
            }
        }

        private void SmoothBackgroundTransition()
        {
            if (backgroundCanvas != null)
            {
                backgroundCanvas.transform.position = Vector3.Lerp(backgroundCanvas.transform.position, targetBackgroundPosition, moveSpeed * Time.deltaTime);
            }
        }
    }
}
