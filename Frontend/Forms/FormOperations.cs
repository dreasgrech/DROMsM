using System.Drawing;
using System.Windows.Forms;

namespace Frontend
{
    public static class FormOperations
    {
        /// <summary>
        /// Gets a location very close to the supplied control.
        /// IMPORTANT: If apply the returned position to a Form, make sure that that form's StartPosition is FormStartPosition.Manual
        /// </summary>
        /// <param name="form"></param>
        /// <param name="relativeControl"></param>
        public static Point GetRelativeControlPoint(Form form, Control relativeControl)
        {
            var locationOnScreen = form.PointToScreen(relativeControl.Location);
            return new Point(locationOnScreen.X, locationOnScreen.Y + relativeControl.Height + 3);
        }
    }
}