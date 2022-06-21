using System.Drawing;
using System.Linq;
using System.Text;

namespace TimeCollapse.Models
{
    public static class MapConstructorSerializer
    {
        /// <summary>
        ///     Метод, преобразующий объект класса Map в сжатую закодированную строку с возможностью восстановления
        /// </summary>
        /// <param name="map">
        ///     Объект класса Map, поля которого удовлетворяют требованиям методов
        ///     MapConstructorSerializer.SerializeRectangle и MapConstructorSerializer.SerializeStage
        /// </param>
        /// <returns></returns>
        public static string SerializeMap(Map map)
        {
            var sb = new StringBuilder(map.Name);
            sb.Append(';');
            foreach (var block in map.Blocks)
                sb.Append(SerializeRectangle(block) + ",");
            sb.Remove(sb.Length - 1, 1);
            sb.Append(';');
            foreach (var anomaly in map.TimeAnomalies)
                sb.Append(SerializeRectangle(anomaly) + ",");
            sb.Remove(sb.Length - 1, 1);
            sb.Append(';');
            foreach (var stage in map.Stages)
            {
                var (s, t) = SerializeStage(stage);
                sb.Append(s + ".");
                sb.Append(t + ",");
            }

            sb.Remove(sb.Length - 1, 1);

            return sb.ToString();
        }

        /// <summary>
        ///     Метод, восстанавливающий из строки, созданной методом MapConstructorSerializer.SerializeMap, объект класса Map
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static Map DeserializeMap(string str)
        {
            var tokens = str.Split(';').Select(t => t.Split(',')).ToArray();
            var name = tokens[0][0];
            var blocks = tokens[1]
                .Select(s => DeserializeRectangle(int.Parse(s)));
            var timeAnomalies = tokens[2]
                .Select(s => DeserializeRectangle(int.Parse(s)));
            var stages = tokens[3]
                .Select(s => s.Split('.'))
                .Select(s => DeserializeStage(int.Parse(s[0]), int.Parse(s[1])));
            return new Map(name, blocks, timeAnomalies, stages);
        }

        /// <summary>
        ///     Метод, преобразующий прямоугольник в целое число в диапозоне от 0 до 214358880, с возможностью восстановления
        /// </summary>
        /// <param name="rect">Прямоугольник, c параметрами, выраженными целыми числами, кратными 16, в диапазоне от 0 до 1920</param>
        /// <returns></returns>
        public static int SerializeRectangle(Rectangle rect)
        {
            var height = rect.Height / 16;
            var width = rect.Width / 16;
            var y = rect.Y / 16;
            var x = rect.X / 16;
            return ((height * 121 + width) * 121 + y) * 121 + x;
        }

        /// <summary>
        ///     Метод, преобразующий число в прямоугольник с параметрами, выраженными целыми числами, кратными 16, в диапазоне от 0
        ///     до 1920
        /// </summary>
        /// <param name="rectCode">Целое число, созданное методом MapConstructorSerializer.SerializeRectangle</param>
        /// <returns></returns>
        public static Rectangle DeserializeRectangle(int rectCode)
        {
            var x = rectCode % 121;
            var y = (rectCode /= 121) % 121;
            var width = (rectCode /= 121) % 121;
            var height = rectCode / 121 % 121;
            return new Rectangle(x * 16, y * 16, width * 16, height * 16);
        }

        /// <summary>
        ///     Метод, преобразующий объект класса Stage в два целых числа в диапозоне от 0 до 214358880, с возможностью
        ///     восстановления
        /// </summary>
        /// <param name="stage">
        ///     Объект класса Stage, параметры полей Spawn и Target которого выражены целыми числами, кратными 16, в диапазоне от 0
        ///     до 1920
        /// </param>
        /// <returns></returns>
        public static (int, int) SerializeStage(Stage stage)
        {
            var spawnY = stage.Spawn.Y / 16;
            var spawnX = stage.Spawn.X / 16;
            return (spawnY * 121 + spawnX, SerializeRectangle(stage.Target));
        }

        /// <summary>
        ///     Метод,преобразующий два числа в Объект класса Stage, параметры полей Spawn и Target которого выражены целыми
        ///     числами, кратными 16, в диапазоне от 0 до 1920
        /// </summary>
        /// <param name="code1">Первое целое число, созданное методом MapConstructorSerializer.SerializeStage</param>
        /// <param name="code2">Второе целое число, созданное методом MapConstructorSerializer.SerializeStage</param>
        /// <returns></returns>
        public static Stage DeserializeStage(int code1, int code2)
        {
            var spawnX = code1 % 121;
            var spawnY = code1 / 121 % 121;
            return new Stage(new Point(spawnX * 16, spawnY * 16), DeserializeRectangle(code2));
        }
    }
}