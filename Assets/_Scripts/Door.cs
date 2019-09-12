using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TE
{
    public class Door : MonoBehaviour, ITimeTravel
    {
        public Vector2 down;
        private Vector2 up;

        [HideInInspector]
        public bool isDown = false;

        bool savedDown;
        // Start is called before the first frame update
        void Start()
        {
            up = new Vector2(transform.localPosition.x, transform.localPosition.y);

        }

        public void MoveDoor(bool down)
        {
            if (down)
            {
                Debug.Log("Moved Door Down");
                StartCoroutine(MoveTo(this.down));
                isDown = true;
            }
            else
            {
                Debug.Log("Moved Door Up");
                StartCoroutine(MoveTo(up));
                isDown = false;
            }
        }

        IEnumerator MoveTo(Vector2 target)
        {
            while (Mathf.Abs(transform.localPosition.y - target.y) > 0.01f)
            {
                transform.localPosition = Vector2.MoveTowards(new Vector2(transform.localPosition.x, transform.localPosition.y),
                    target, 3f * Time.deltaTime);
                yield return new WaitForEndOfFrame();
            }
            yield return true;

        }

        public void HandleTimeStamp()
        {
            savedDown = isDown;
        }

        public void HandleTimeTravel()
        {
            MoveDoor(savedDown);
        }
    }
}
