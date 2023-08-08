using Newtonsoft.Json;
using System.Text;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;

namespace IotCoreDemo.Utils
{
    public class Listeners
    {
        private readonly MqttClient _client;
        public Listeners(MqttClient client)
        {
            _client = client;
        }
        public void RespondNotification(object sender, MqttMsgPublishEventArgs e)
        {
            var topic = e.Topic.Split('/');
            if (topic[2] == "notifications")
            {
                var command = new
                {
                    message = $"Notification from device {topic[3]} delivered to API and saved to Database."
                };
                _client.Publish($"/stations/status-report", Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(command)));
            }else if (topic[2] == "alerts")
            {
                var command = new
                {
                    message = $"Alert from device {topic[3]} delivered to API and saved to Database."
                };
                _client.Publish($"/stations/status-report", Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(command)));
            }
            else if (topic[2] == "info")
            {
                var command = new
                {
                    message = $"Information from device {topic[3]} delivered to API and saved to Database."
                };
                _client.Publish($"/stations/status-report", Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(command)));
            }

        }
    }
}
