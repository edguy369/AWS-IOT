using DataAccess.Config;
using IotCoreDemo.Utils;
using System.Security.Cryptography.X509Certificates;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.ConfigureDataAccessLayer(builder.Configuration);


var broker = "a60l4gfegqa2k-ats.iot.us-east-2.amazonaws.com"; //<AWS-IoT-Endpoint>           
var port = 8883;
var clientId = "API";
var certPass = "C@d1ngT1p1";

var certificatesPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "certs");

var caCertPath = Path.Combine(certificatesPath, "AmazonRootCA1.pem");
var caCert = X509Certificate.CreateFromCertFile(caCertPath);

var deviceCertPath = Path.Combine(certificatesPath, "certificate.cert.pfx");
var deviceCert = new X509Certificate(deviceCertPath, certPass);

// Create a new MQTT client.
var client = new MqttClient(broker, port, true, caCert, deviceCert, MqttSslProtocols.TLSv1_2);
var listeners = new Listeners(client);
client.MqttMsgPublishReceived += listeners.RespondNotification;
//Connect
client.Connect(clientId);

builder.Services.AddSingleton(client);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
