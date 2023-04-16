using System;
using System.Collections;
using System.Net;
using UnityEngine;
using UnityEngine.Networking;
public class httpClient : MonoBehaviour
{
    HttpListener _httpListener = new HttpListener();
    Vector3 newRotation = new Vector3(0,0,0);
    Vector3 initRotation = new Vector3(0,0,0);

    // WebSocket ws;
    void Start()
	{
		StartCoroutine(getRequest("http://169.231.44.118:5000"));
	}

	void Update(){

		// Important to adjust the initial angle when phone's upright
		if (Input.GetKeyDown(KeyCode.A))
		{
			initRotation = newRotation;
		}
	}
	
	private void setOrientation(string angles)
    {
        // Debug.Log(angles);

        try{

            string[] arr = angles.Split(',');
            float y = float.Parse( arr[0]);
            float x = float.Parse( arr[1]);
            float z = float.Parse( arr[2]);
            // Debug.Log(x.ToString()+", "+y.ToString()+", "+z.ToString());
            
            newRotation = new Vector3(x,y,z);
			transform.eulerAngles = newRotation - initRotation;
        }
        catch(Exception e){
            Debug.Log(e);
        }
        
    }

	IEnumerator getRequest(string uri)
	{
		while(true){
			UnityWebRequest uwr = UnityWebRequest.Get(uri);
			yield return uwr.SendWebRequest();

			if (uwr.isNetworkError)
			{
				Debug.Log("Error While Sending: " + uwr.error);
			}
			else
			{
				// Debug.Log("Received: " + uwr.downloadHandler.text);
				setOrientation(uwr.downloadHandler.text);
			}

			// yield return new WaitForSeconds(0.02f);
		}
	}
}
