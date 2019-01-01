using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleARCore;

public class AnchorMap : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
        // if player has not touched the screen, we are done with this update
        Touch touch;
        if (Input.touchCount < 1 || (touch = Input.GetTouch(0)).phase != TouchPhase.Began)
        {
            return;
        }

        // rawcast against the point where player touched 
        TrackableHit hit;

        TrackableHitFlags raycastFilter = TrackableHitFlags.PlaneWithinBounds | TrackableHitFlags.PlaneWithinPolygon | TrackableHitFlags.FeaturePointWithSurfaceNormal;

        if (Frame.Raycast(touch.position.x, touch.position.y, raycastFilter, out hit))
        {
            var anchor = hit.Trackable.CreateAnchor(hit.Pose);
            transform.position = anchor.transform.position;
        }

    }
}
