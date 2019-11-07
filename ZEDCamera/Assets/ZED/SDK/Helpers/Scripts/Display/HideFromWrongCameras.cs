﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Stops an object from being drawn for cameras that shouldn't draw it, without the use of layers. 
/// Used so that the left ZED eye doesn't see the right eye's canvas object, and vice versa, and for 
/// cameras seeing frames from other rigs. 
/// </summary><remarks>
/// These are attached to canvas object in ZED_Rig_Mono and ZED_Rig_Stereo prefabs, and
/// assigned to Quad objects in the (normally hidden) AR rig created by ZEDManager when in stereo AR mode.
/// </remarks
public class HideFromWrongCameras : MonoBehaviour
{
    /// <summary>
    /// List of all cameras from the ZED plugin that should see exactly one frame object. 
    /// <para>Effectively unused if showInNonZEDCameras is set to false.</para>
    /// </summary>
    private static List<Camera> zedCamList = new List<Camera>();

    /// <summary>
    /// If true, the object will be visible to all cameras not in zedCamList, namely cameras
    /// that aren't part of the ZED plugin. 
    /// </summary>
    public bool showInNonZEDCameras = true;

    /// <summary>
    /// The camera that is allowed to render this object. 
    /// </summary>
    private Camera renderableCamera = null;

    /// <summary>
    /// Renderer attached to this object. 
    /// </summary>
    private Renderer rend;

    /// <summary>
    /// Enabled state of the attached Renderer prior to Unity's rendering stage. 
    /// <para>Used so that manually disabling the object's MeshRenderer won't be undone by this script.</para>
    /// </summary>
    private bool lastRenderState = true;

    public void Awake()
    {
        rend = GetComponent<Renderer>();

        Camera.onPreRender += PreRender;
        Camera.onPostRender += PostRender;
    }

    /// <summary>
    /// Adds a camera to an internal static list of cameras that should see exactly one ZED frame/canvas object. 
    /// <para>Also called automatically if you call SetRenderCamera as a backup.</para>
    /// </summary>
    /// <param name="cam"></param>
    public static void RegisterZEDCam(Camera cam)
    {
        if (!zedCamList.Contains(cam))
            zedCamList.Add(cam);
    }

    /// <summary>
    /// Assigns a camera to this object, making it the only camera of the registered cameras that can see it. 
    /// </summary>
    /// <param name="cam"></param>
    public void SetRenderCamera(Camera cam)
    {
        RegisterZEDCam(cam); //In case it's not registered yet, make sure it is. 
        renderableCamera = cam;
    }
    
    /// <summary>
    /// Turns the renderer on or off depending on whether it should be drawn. 
    /// <para>Subscribed to Camera.onPreRender in Awake(), which is called anytime any camera starts rendering.</para>
    /// </summary>
    /// <param name="currentcam"></param>
    private void PreRender(Camera currentcam)
    {
        lastRenderState = rend.enabled;
        if (!rend.enabled) return; //We weren't going to render this object anyway, so skip the rest of the logic. 

        //If it's a Unity scene camera, always show it. 
        if (currentcam.name.ToLower().Contains("scenecamera")) //There are more robust ways of checking this, but they are expensive. 
        {
            rend.enabled = true; 
            return;
        }

        //Is it the renderable camera we assigned to this instance?
        if (currentcam == renderableCamera)
        {
            rend.enabled = true; //Always render that camera, no matter what. 
        }
        else if(zedCamList.Contains(currentcam)) //Is it included in the ZED cameras that should only see one of these objects?
        {
            rend.enabled = false; //Those cameras can only render one quad, and it's not this one. 
        }
        else  //If it's a camera not on the list, render depending on showInNonZEDCameras. 
        {
            rend.enabled = showInNonZEDCameras; 
        }

    }

    /// <summary>
    /// Sets the renderer's state to what it was before PreRender() messed with it. 
    /// <para>Subscribed to Camera.onPostRender in Awake(), which is called anytime any camera finishes rendering.</para>
    /// </summary>
    private void PostRender(Camera currentcam)
    {
        rend.enabled = lastRenderState;
    }

    private void OnDestroy()
    {
        Camera.onPreRender -= PreRender;
        Camera.onPostRender -= PostRender;
    }

}
