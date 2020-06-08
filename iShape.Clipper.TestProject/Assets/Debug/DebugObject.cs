using Debug;
using iShape.Clipper.Collision;
using iShape.Clipper.Collision.Primitive;
using iShape.Clipper.Solver;
using iShape.Geometry;
using Unity.Collections;
using UnityEngine;

public class DebugObject : MonoBehaviour {

    private const Allocator allocator = Allocator.TempJob;
    
    public void DoMemoryLeak() {
        const int n = 10;
        var pointer = new NativeArray<int>(10, Allocator.TempJob);
        for (int i = 0; i < n; ++i) {
            pointer[i] = i;
        }
    }
    
    public void DoMemoryLeakTest() {

        NativeLeakDetection.Mode = NativeLeakDetectionMode.EnabledWithStackTrace;
        this.PinTests();
        this.IntersectTests();
        this.SubtractTests();
        this.UnionTests();
        this.BiteTests();
    }

    private void PinTests() {
        int n = PinTestData.data.Length;
        for (int i = 0; i < n; ++i) {
            var data = PinTestData.data[i];

            var iMaster = new NativeArray<IntVector>(IntGeom.DefGeom.Int(data[0]), allocator);
            var iSlave = new NativeArray<IntVector>(IntGeom.DefGeom.Int(data[1]), allocator);

            var result = CrossDetector.FindPins(iMaster, iSlave, PinPoint.PinType.nil, allocator);

            result.Dispose();
            iMaster.Dispose();
            iSlave.Dispose();            
        }
    }

    private void IntersectTests() {
        int n = SubtractTestData.data.Length;
        for (int i = 0; i < n; ++i) {

            var data = SubtractTestData.data[i];

            var master = new NativeArray<IntVector>(IntGeom.DefGeom.Int(data[0]), allocator);
            var slave = new NativeArray<IntVector>(IntGeom.DefGeom.Int(data[1]), allocator);

            var solution = master.Intersect(slave, allocator);

            solution.Dispose();
            master.Dispose();
            slave.Dispose(); 
        }
    }
    
    private void SubtractTests() {
        int n = SubtractTestData.data.Length;
        for (int i = 0; i < n; ++i) {

            var data = SubtractTestData.data[i];

            var master = new NativeArray<IntVector>(IntGeom.DefGeom.Int(data[0]), allocator);
            var slave = new NativeArray<IntVector>(IntGeom.DefGeom.Int(data[1]), allocator);

            var solution = master.Subtract(slave, allocator);

            solution.Dispose();
            master.Dispose();
            slave.Dispose(); 
        }
    }
    
    private void UnionTests() {
        int n = UnionTestData.data.Length;
        for (int i = 0; i < n; ++i) {

            var data = UnionTestData.data[i];

            var master = new NativeArray<IntVector>(IntGeom.DefGeom.Int(data[0]), allocator);
            var slave = new NativeArray<IntVector>(IntGeom.DefGeom.Int(data[1]), allocator);

            var solution = master.Union(slave, allocator);

            solution.Dispose();
            master.Dispose();
            slave.Dispose(); 
        }
    }
    
    private void BiteTests() {
        int n = BiteTestData.data.Length;
        for (int i = 0; i < n; ++i) {
            var data = BiteTestData.data[0].Allocate(allocator);
            var solution = data.shape.ComplexSubtract(data.path, allocator);
            data.Dispose();
            solution.Dispose();
        }
    }

}
