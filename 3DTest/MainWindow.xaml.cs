using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace _3DTest
{
    public partial class MainWindow : Window
    {
        BackgroundWorker worker = new BackgroundWorker();
        AxisAngleRotation3D rotateAxis;

        public MainWindow()
        {
            InitializeComponent();

            // fps counter
            CompositionTarget.Rendering += CompositionTarget_Rendering;

            PerspectiveCamera camera = new PerspectiveCamera();
            camera.Position = new Point3D(0, 0, 2);
            camera.LookDirection = new Vector3D(0, 0, -1);
            camera.FieldOfView = 90;
            viewport.Camera = camera;

            // Lighting
            DirectionalLight directionalLight = new();
            directionalLight.Color = Colors.White;
            directionalLight.Direction = new Vector3D(-0.61, -0.5, -0.61);


            // model
            ModelVisual3D modelVisual3D = new ModelVisual3D();
            viewport.Children.Add(modelVisual3D);

            Model3DGroup modelGroup = new();
            modelGroup.Children.Add(directionalLight);
            modelVisual3D.Content = modelGroup;

            // geometry
            GeometryModel3D geometry = new();
            MeshGeometry3D mesh = new();

            Point3DCollection point3Ds = new();
            point3Ds.Add(new Point3D(-0.5, -0.5, 0.5));
            point3Ds.Add(new Point3D(0.5, -0.5, 0.5));
            point3Ds.Add(new Point3D(0.5, 0.5, 0.5));
            point3Ds.Add(new Point3D(0.5, 0.5, 0.5));
            point3Ds.Add(new Point3D(-0.5, 0.5, 0.5));
            point3Ds.Add(new Point3D(-0.5, -0.5, 0.5));

            Int32Collection triangleIndices = new ();
            triangleIndices.Add(0);
            triangleIndices.Add(1);
            triangleIndices.Add(2);
            triangleIndices.Add(3);
            triangleIndices.Add(4);
            triangleIndices.Add(5);

            mesh.Positions = point3Ds;
            mesh.TriangleIndices = triangleIndices;
            geometry.Geometry = mesh;
            modelGroup.Children.Add(geometry);

            // material
            LinearGradientBrush gradientBrush = new();
            gradientBrush.StartPoint = new Point(0, 0.5);
            gradientBrush.EndPoint = new Point(1, 0.5);
            gradientBrush.GradientStops.Add(new GradientStop(Colors.Yellow, 0.0));
            gradientBrush.GradientStops.Add(new GradientStop(Colors.Red, 0.25));
            gradientBrush.GradientStops.Add(new GradientStop(Colors.Blue, 0.75));
            gradientBrush.GradientStops.Add(new GradientStop(Colors.Green, 0.76));

            DiffuseMaterial material = new DiffuseMaterial(gradientBrush);
            geometry.Material = material;

            // transform
            RotateTransform3D rotateTransform = new ();
            this.rotateAxis = new ();
            this.rotateAxis.Axis = new Vector3D(0, 3, 0);
            this.rotateAxis.Angle = 40;
            rotateTransform.Rotation = rotateAxis;
            geometry.Transform = rotateTransform;

            worker.WorkerReportsProgress = true;
            worker.DoWork += Worker_DoWork;
            worker.ProgressChanged += Worker_ProgressChanged;
            worker.RunWorkerAsync();
        }

        int fps = 0;
        private void CompositionTarget_Rendering(object sender, EventArgs e)
        {
            fps += 1;
            this.rotateAxis.Angle += 1;
        }

        private void Worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            lblFPS.Content = "FPS: " + fps;
            fps = 0;
        }

        private void Worker_DoWork(object sender, DoWorkEventArgs e)
        {
            while(true)
            {
                worker.ReportProgress(10);
                Thread.Sleep(1000);
            }
        }
    }
}
