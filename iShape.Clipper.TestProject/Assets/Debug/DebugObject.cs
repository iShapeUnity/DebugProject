using Debug;
using iShape.Clipper.Collision;
using iShape.Clipper.Collision.Primitive;
using iShape.Clipper.Solver;
using iShape.Geometry;
using Unity.Collections;
using UnityEngine;

public class DebugObject : MonoBehaviour {

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
    }

    private void PinTests() {
        int n = PinTestData.data.Length;
        for (int i = 0; i < n; ++i) {
            var data = PinTestData.data[i];

            var iMaster = new NativeArray<IntVector>(IntGeom.DefGeom.Int(data[0]), Allocator.Temp);
            var iSlave = new NativeArray<IntVector>(IntGeom.DefGeom.Int(data[1]), Allocator.Temp);

            var result = CrossDetector.FindPins(iMaster, iSlave, IntGeom.DefGeom, PinPoint.PinType.nil, Allocator.TempJob);

            result.Dispose();
            iMaster.Dispose();
            iSlave.Dispose();            
        }
    }

    private void IntersectTests() {
        int n = SubtractTestData.data.Length;
        for (int i = 0; i < n; ++i) {

            var data = SubtractTestData.data[i];

            var master = new NativeArray<IntVector>(IntGeom.DefGeom.Int(data[0]), Allocator.Temp);
            var slave = new NativeArray<IntVector>(IntGeom.DefGeom.Int(data[1]), Allocator.Temp);

            var solution = master.Intersect(slave, IntGeom.DefGeom, Allocator.TempJob);

            solution.Dispose();
            master.Dispose();
            slave.Dispose(); 
        }
    }
    
    private void SubtractTests() {
        int n = SubtractTestData.data.Length;
        for (int i = 0; i < n; ++i) {

            var data = SubtractTestData.data[i];

            var master = new NativeArray<IntVector>(IntGeom.DefGeom.Int(data[0]), Allocator.Temp);
            var slave = new NativeArray<IntVector>(IntGeom.DefGeom.Int(data[1]), Allocator.Temp);

            var solution = master.Subtract(slave, IntGeom.DefGeom, Allocator.TempJob);

            solution.Dispose();
            master.Dispose();
            slave.Dispose(); 
        }
    }
    
    private void UnionTests() {
        int n = UnionTestData.data.Length;
        for (int i = 0; i < n; ++i) {

            var data = UnionTestData.data[i];

            var master = new NativeArray<IntVector>(IntGeom.DefGeom.Int(data[0]), Allocator.Temp);
            var slave = new NativeArray<IntVector>(IntGeom.DefGeom.Int(data[1]), Allocator.Temp);

            var solution = master.Union(slave, IntGeom.DefGeom, Allocator.TempJob);

            solution.Dispose();
            master.Dispose();
            slave.Dispose(); 
        }
    }

}
