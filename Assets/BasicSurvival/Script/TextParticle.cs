using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextParticle : MonoBehaviour {

    TextMesh textMesh;
    public float speed = 0.2f;
    Transform camera;


    void Start () {
        textMesh = GetComponent<TextMesh>();
        camera = Camera.main.transform;
        StartCoroutine(destroyText());
    }
	
	// Update is called once per frame
	void Update () {
        textMesh.transform.LookAt(camera);
        transform.localRotation *= Quaternion.Euler(0, 180, 0);

        Vector3 pos = transform.position;
        pos.y += speed;
        transform.position = pos;       
	}

    public void SetText(string text)
    {
        if (textMesh == null)
            textMesh = GetComponent<TextMesh>();
        this.textMesh.text = text;
    }

    public void SetText(string text, Color color)
    {
        if (textMesh == null)
            textMesh = GetComponent<TextMesh>();
        this.textMesh.text = text;
        this.textMesh.color = color;
    }

    IEnumerator destroyText()
    {
        yield return new WaitForSeconds(2);
        Destroy(gameObject);
    }
}
