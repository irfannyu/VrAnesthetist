using System.Collections;
using System.Collections.Generic;
using TMPro; // Make sure to include this namespace
using UnityEngine;
using UnityEngine.XR;

public class ObjectRotator : MonoBehaviour
{
    // This code deals with the rotation and the position only
    // syringeTipPosition is the coorect one
    Vector3 syringeTipPosition;

    public GameObject actualsyringePosition;

    // This is the distance to the point
    float injectionThreshold = 1.0f;

    float x = 0.0f, y = 0.0f, z = 0.0f;

    float xActual = 0f, yActual = 0f, zActual = 0f;

    // This tolerance is for angle
    float tolerance = 30.0f;

    bool injecting = false;

    // This stores the amount injected
    float injectionAmount = 0.0f;

    // This is the total amount to be injected
    float injectionAmountThreshold = 21.3f;

    // To show instructions on GUI
    public TextMeshProUGUI TextRotationGUI;

    public TextMeshProUGUI TexPositionGUI;

    string displayTextRotation = "";

    string displayTextDepth = "";

    void Update()
    {
        // Set the actual syringe position
        syringeTipPosition = actualsyringePosition.transform.position;

        // Get the game object's rotation as a Quaternion
        Quaternion rotation = gameObject.transform.rotation;

        // Convert the Quaternion rotation to Euler angles
        Vector3 Angles = rotation.eulerAngles;

        // Access the individual x, y, and z rotation values
        x = Angles.x;
        y = Angles.y;
        z = Angles.z;

        // Print the rotation values to the console
        float diffx = x - xActual;
        float diffy = y - yActual;
        float diffz = z - zActual;

        displayTextRotation = "";
        if (Mathf.Abs(diffx) > tolerance)
        {
            displayTextRotation +=
                ("Move x in " + diffx.ToString("0.00") + " angle.\n");
        }

        if (Mathf.Abs(diffy) > tolerance)
        {
            displayTextRotation +=
                ("Move y in " + diffy.ToString("0.00") + " angle.\n");
        }

        if (Mathf.Abs(diffz) > tolerance)
        {
            displayTextRotation +=
                ("Move z in " + diffz.ToString("0.00") + " angle.\n");
        }

        TextRotationGUI.text = displayTextRotation;

        // Check the position
        // Print the rotation values to the console
        float diffPosition =
            Vector3.Distance(syringeTipPosition, gameObject.transform.position);
        if (diffPosition > injectionThreshold && Mathf.Abs(diffx) <= tolerance && Mathf.Abs(diffy) <= tolerance && Mathf.Abs(diffz) <= tolerance)
        {
            TexPositionGUI.text =
                ("Push down " + diffPosition.ToString("0.00") + " more.");
        }

        // Now detect button Press
        if (Mathf.Abs(diffx) < tolerance && Mathf.Abs(diffy) < tolerance && Mathf.Abs(diffz) < tolerance && diffPosition <= injectionThreshold)
        {
            if (OVRInput.GetDown(OVRInput.Button.One, OVRInput.Controller.RTouch))
            {

                // Toggle the injection with button press
                if (!injecting) injecting = !injecting;
            }
            else
            {
                if (injecting && injectionAmount < injectionAmountThreshold)
                {
                    TexPositionGUI.text = "Continue Injection.";
                    TextRotationGUI.text =
                        injectionAmount.ToString("0.00") +
                        "/" +
                        injectionAmountThreshold.ToString("0.00") +
                        " injected.";
                }
                else if (injecting && injectionAmount >= injectionAmountThreshold)
                {
                    TexPositionGUI.text = "Stop Injection.";
                    TextRotationGUI.text =
                        injectionAmount.ToString("0.00") +
                        "/" +
                        injectionAmountThreshold.ToString("0.00") +
                        " injected.";
                }
                else if (
                    !injecting &&
                    injectionAmount <= injectionAmountThreshold
                )
                {
                    TexPositionGUI.text = "Begin Injection.";
                    TextRotationGUI.text =
                        injectionAmount.ToString("0.00") +
                        "/" +
                        injectionAmountThreshold.ToString("0.00") +
                        " injected.";
                }
                else
                {
                    TexPositionGUI.text = "Injection Complete.";
                    TextRotationGUI.text =
                        injectionAmount.ToString("0.00") +
                        "/" +
                        injectionAmountThreshold.ToString("0.00") +
                        " injected.";
                }
            }
        }

        // Increment the amount
        if (injecting)
        {
            injectionAmount += 0.7f * Time.deltaTime;
        }
    }
}
