using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MiniJSON;
using Mapbox.Unity.Map;
using TMPro;


public class Instagramgetter : MonoBehaviour
{
    public AbstractMap _map;
    private double lat;
    private double lon;
    private GameObject instaPost;

    // Start is called before the first frame update
    IEnumerator Start()
    {
        string url = "https://api.instagram.com/v1/users/self/media/recent/?access_token=4355211949.0a435af.1e6998d744b04b7cadef566bc5a772d4";

        // The above has been depricated
        WWW www = new WWW(url);
        yield return www;

        string api_response = www.text;
        Debug.Log(api_response);

        IDictionary apiParse = (IDictionary)Json.Deserialize(api_response);
        IList instagramPicturesList = (IList)apiParse["data"];
        
        // We need to initalize the map programmatically to show the map
        // We can't use the checkbox for this.
        _map.Initialize(new Mapbox.Utils.Vector2d(41.82720257228, -71.41640192116), 15);
        

        foreach (IDictionary instagramPicture in instagramPicturesList)
        {
            // Main picture info
            IDictionary images = (IDictionary)instagramPicture["images"];
            IDictionary standardResolution = (IDictionary)images["standard_resolution"];
            string mainPic_url = (string)standardResolution["url"];
            Debug.Log(mainPic_url);

            WWW mainPic = new WWW(mainPic_url);
            yield return mainPic;

            // Location info
            IDictionary location = (IDictionary)instagramPicture["location"];
        
            if (location != null)
            {
                lat = (double)location["latitude"];
                lon = (double)location["longitude"];

                instaPost = Instantiate(Resources.Load("InstaPost")) as GameObject;
                instaPost.transform.GetChild(0).GetComponent<MeshRenderer>().material.mainTexture = mainPic.texture;
                instaPost.transform.position = _map.GeoToWorldPosition(new Mapbox.Utils.Vector2d(lat, lon)) + new Vector3(0,0.15f,0);
                instaPost.transform.SetParent(_map.transform);


                IDictionary user = (IDictionary)instagramPicture["user"];
                string userName = (string)user["username"];
                string profilePicture_url = (string)user["profile_picture"];

                WWW instaProfilePic = new WWW(profilePicture_url);
                yield return instaProfilePic;

                instaPost.transform.GetChild(1).GetComponent<MeshRenderer>().material.mainTexture = instaProfilePic.texture;
                instaPost.transform.GetChild(2).GetComponent<TextMeshPro>().text = userName;

                // Location name
                string placeName = (string)location["name"];
                instaPost.transform.GetChild(3).GetComponent<TextMeshPro>().text = placeName;

                // Likes
                IDictionary Likes = (IDictionary)instagramPicture["likes"];
                string likes = (string)Likes["count"].ToString();

                instaPost.transform.GetChild(4).GetComponent<TextMeshPro>().text = likes + " likes";

                // Captions 
                IDictionary Caption = (IDictionary)instagramPicture["caption"];

                if (Caption != null)
                {
                    string caption = (string)Caption["text"];

                    instaPost.transform.GetChild(5).GetComponent<TextMeshPro>().text = caption;
                }

            }


        }

        _map.transform.position = new Vector3(0, 5, 0);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
