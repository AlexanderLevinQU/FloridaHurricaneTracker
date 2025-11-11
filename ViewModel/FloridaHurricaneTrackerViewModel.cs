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
        private ObservableCollection<Hurricane> _cobjHurricanes = new ObservableCollection<Hurricane>();

        public ObservableCollection<Hurricane> Hurricanes
        {
            get => _cobjHurricanes;
            set => SetProperty(ref _cobjHurricanes, value);
        }

        public FloridaHurricaneTrackerViewModel()
        {
            FloridaPolygonWriter floridaPolygonWriter = new FloridaPolygonWriter(); // Can change this to a polygon writer and be more general
            floridaPolygonWriter.CreateFloridaPolygon();
            HurricaneParser hurricaneParser = new HurricaneParser();
            _cobjHurricanes = new ObservableCollection<Hurricane> (hurricaneParser.ParseHurricanes());
        }

    }
}
