using System.Threading;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class ChunkLoader : MonoBehaviour {

	Thread thread;
	AutoResetEvent resetEvent;

	Queue<Chunk> data;
	Queue<Chunk> output;
	Chunk chunkRender;
	long idMark;

	bool isStopped;
	bool needLoad;

	void Awake () {

		this.data = new Queue<Chunk>(65536);
		this.output = new Queue<Chunk>(65536);

		this.idMark = 0;

		this.isStopped = false;
		this.needLoad = false;

		this.resetEvent = new AutoResetEvent(false);

		this.thread = new Thread (ThreadWorker);
		this.thread.Start ();
	}

	void Update() {

		lock (this.output) {    //Render chunk from queue

			while (this.output.Count != 0) {   //Queue empty

				//Render chunk
				//float time = Time.realtimeSinceStartup;
				this.chunkRender = this.output.Dequeue();
				this.chunkRender.ApplyMesh();
				this.chunkRender.ApplyNeighborsMesh();
				//Debug.Log(Time.realtimeSinceStartup - time);
			}
		}

	}

	void ThreadWorker() {

		Queue<Chunk> temp = new Queue<Chunk> (65536);
		Chunk chunkInit = null;
		

		while (!this.isStopped) {

			lock (this.data) {
				if (this.data.Count != 0) {	//Get chunk from queue
					chunkInit = this.data.Dequeue();
				} else {
					chunkInit = null;
				}
			}

			if (chunkInit == null) {	//Start update mesh if queue empty - it mean all chunk filled with block

				while (true) {

					if (temp.Count == 0) {
						break;
					}

					chunkInit = temp.Dequeue();
					chunkInit.CaculateMesh();
					chunkInit.CaculateNeighborsMesh();

					lock (this.output) {
						this.output.Enqueue(chunkInit);
					}
				}

				this.needLoad = false;
				//UnityEngine.Debug.Log("Finish");
				this.resetEvent.WaitOne();
				continue;
			}

			//Fill chunk with block
			chunkInit.Fill ();
			chunkInit.SetLoadedEvent();

			//Push chunk filled for update mesh
			temp.Enqueue(chunkInit);
		}
	}

	public void AddChunk(Chunk chunk) {

		this.needLoad = true;
		lock (this.data) {
			this.data.Enqueue (chunk);
		}
	}

	public void InitLoader() {

		lock (this.data) {
			this.data.Clear();
		}
		lock (this.output) {
			this.output.Clear();
		}
	}

	public void Load() {

		if (this.needLoad) {
			this.resetEvent.Set();  //Wake up thread for load chunk
		}
	}



	void OnDestroy (){

		this.thread.Abort();
		this.isStopped = true;
	}

	void OnApplicationQuit () {

		this.thread.Abort();
		this.isStopped = true;
	}
}
