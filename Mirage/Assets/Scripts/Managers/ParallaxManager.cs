using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// manages parallax effect of background scenery (snow and stars)
public class ParallaxManager : MonoBehaviour
{
    // reference to array of all the backgrounds to be parallaxed
    public Transform[] backgrounds;

    // reference to scales relative to camera's movement to move the backgrounds by
    private float[] _parallaxScales; 

    // reference to smoothing factor
    public float smoothing = 1f; 

    // reference to main camera transform
    private Transform _camera;

    // reference to camera position in the previous frame 
    private Vector3 _previousCamPos; 
                                    
    void Awake()
    {
        _camera = Camera.main.transform;
    }

    // initializes initial camera position and background scales
    void Start()
    {
        _previousCamPos = _camera.position;

        _parallaxScales = new float[backgrounds.Length];
        for (int i = 0; i < backgrounds.Length; i++)
        {
            _parallaxScales[i] = backgrounds[i].position.z * 1;
        }
    }

    // every frame calcules new background positions relative to camera utilising their scale and lerp to perform this smoothly
    void Update()
    {
        for (int i = 0; i < backgrounds.Length; i++)
        {
            float parallax = (_previousCamPos.x - _camera.position.x) * _parallaxScales[i];

            float backgroundTargetPosX = backgrounds[i].position.x + parallax;

            Vector3 backgroundTargetPos = new Vector3(backgroundTargetPosX, backgrounds[i].position.y, backgrounds[i].position.z);

            backgrounds[i].position = Vector3.Lerp(backgrounds[i].position, backgroundTargetPos, smoothing * Time.deltaTime);
        }

        _previousCamPos = _camera.position;
    }
}