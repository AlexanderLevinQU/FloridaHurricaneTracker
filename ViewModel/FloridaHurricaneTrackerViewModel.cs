using FloridaHurricaneTracker.Model;
using FloridaHurricaneTracker.Model.HurricaneInfo;
using FloridaHurricaneTracker.Model.Polygons;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FloridaHurricaneTracker.ViewModel
{
    internal class FloridaHurricaneTrackerViewModel : ViewModelBase
    {
        private ObservableCollection<Hurricane> _hurricanes = new ObservableCollection<Hurricane>();

        public ObservableCollection<Hurricane> Hurricanes
        {
            get => _hurricanes;
            set => SetProperty(ref _hurricanes, value);
        }

        public FloridaHurricaneTrackerViewModel()
        {
            FloridaPolygonWriter floridaPolygonWriter = new FloridaPolygonWriter(); // Can change this to a polygon writer and be more general
            floridaPolygonWriter.CreateFloridaPolygon();
            HurricaneParser hurricaneParser = new HurricaneParser();
            _hurricanes = new ObservableCollection<Hurricane> (hurricaneParser.ParseHurricanes());
        }

    }
}
