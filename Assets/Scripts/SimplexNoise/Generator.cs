using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class Generator : MonoBehaviour {

	public InputField worldX;
	public InputField worldZ;
	public InputField seed;

	public Text output;
	public Text sizeOnDisk;

	public Button generateBtn;


	Thread thread;
	AutoResetEvent resetEvent;

	bool isStopped;
	bool update;
	bool done;

	long count;
	long total;

	int seed_number;


	void Awake() {

		this.isStopped = false;
		this.update = false;
		this.resetEvent = new AutoResetEvent(false);

		this.thread = new Thread(ThreadWorker);
		this.thread.Start();
	}

	void Start () {

		//Debug.Log(sizeof(short));

		this.worldX.onValueChanged.AddListener(delegate { CalculateSizeInDisk(); });
		this.worldZ.onValueChanged.AddListener(delegate { CalculateSizeInDisk(); });
		this.generateBtn.onClick.AddListener(delegate { Generate(); });
	}
	
	
	void Update () {
		
		if (this.update) {

			this.output.text = ((this.count / (float)this.total) * 100).ToString("F2") + " %";
			this.update = false;
		} else if (this.done) {

			if (this.count == 0) {
				this.output.text = "Done! Seed has been planted";
			} else {
				this.output.text = "Done!";
			}
			this.generateBtn.interactable = true;
			this.done = false;
		}

	}

	void Generate() {

		if (this.worldX.text != "" && this.worldZ.text != "" && this.seed.text != "") {

			this.seed_number = int.Parse(this.seed.text);

			Noise.Seed(this.seed_number);

			this.resetEvent.Set();
			this.generateBtn.interactable = false;
		}
	}

	void CalculateSizeInDisk() {

		if (this.worldX.text != "" && this.worldZ.text != "") {

			float size = ((int.Parse(this.worldX.text) * 16 * int.Parse(this.worldZ.text) * 16) / (float)1048576) * 2;
			this.sizeOnDisk.text = size.ToString("F2") + " MB";
		}
	}


	void ThreadWorker() {

		while (!this.isStopped) {

			this.resetEvent.WaitOne();

			int wx = int.Parse(this.worldX.text);
			int wz = int.Parse(this.worldZ.text);
			this.total = wx * wz;

			float step = 1 / (float)3000;
			Vector3 point = new Vector3(0, 0, 0);
			



			if (File.Exists(this.seed_number.ToString())) {

				BinaryReader binaryReader = new BinaryReader(File.Open(this.seed_number.ToString(), FileMode.Open));
				binaryReader.BaseStream.Seek(binaryReader.BaseStream.Length - 4 * 3, SeekOrigin.Begin);
				
				if (binaryReader.ReadInt32() == wx && binaryReader.ReadInt32() == wz && binaryReader.ReadInt32() == this.seed_number) {

					this.count = 0;
					this.done = true;

					binaryReader.Close();
					continue;
				}

				binaryReader.Close();
			}




			this.count = 0;
			this.done = false;

			BinaryWriter binaryWriter = new BinaryWriter(File.Open(this.seed_number.ToString(), FileMode.Create));

			for (int x = 0; x < wx; x++) {
				for (int z = 0; z < wz; z++) {

					point.x = x * 16 * step;
					point.y = z * 16 * step;
					point.z = 0;

					for (int i = 0; i < 16; i++) {
						point.x += step;
						for (int j = 0; j < 16; j++) {
							point.y += step;

							binaryWriter.Write((short)((Noise.Sum(point, 5f, 6, 2f, 0.5f).value + 0.6f) * 128 + 250 * 16));
							
						}
						point.y -= (step * 16);
					}

					this.update = true;
					this.count += 1;
				}
			}

			binaryWriter.Write((int)wx);
			binaryWriter.Write((int)wz);
			binaryWriter.Write((int)this.seed_number);

			this.done = true;

			binaryWriter.Close();
		}
	}


	void OnDestroy() {

		this.thread.Abort();
		this.isStopped = true;
	}

	void OnApplicationQuit() {

		this.thread.Abort();
		this.isStopped = true;
	}
}
