using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;

[BurstCompile]
public struct MemoryLeakJob : IJob {

    private NativeArray<int> array;
    
    public MemoryLeakJob(int n) {
        this.array = new NativeArray<int>(n, Allocator.Temp);
    }

    public void Execute() {
        int n = this.array.Length;
        
        for (int i = 0; i < n; ++i) {
            array[i] = i;
        }

        // array.Dispose();
    }

    public void Dispose() {

    }
}