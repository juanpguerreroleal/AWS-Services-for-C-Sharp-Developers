
using System.Text.Json;
using Amazon.SQS;
using Amazon.SQS.Model;
using SQS.Publisher;

AmazonSQSClient client = new AmazonSQSClient();

var customer = new CustomerCreated
{
    Id = Guid.NewGuid(),
    Email = "juanpabloguleal@gmail.com",
    FullName = "Juan Guerrero",
    DateOfBirth = new DateTime(1997,12,20)
};

var queueUrlResponse = await client.GetQueueUrlAsync("customers");


var sendMessageRequest = new SendMessageRequest()
{
    QueueUrl = queueUrlResponse.QueueUrl,
    MessageBody = JsonSerializer.Serialize(customer),
    MessageAttributes = new Dictionary<string, MessageAttributeValue>()
    {
        {
            "MessageType", new MessageAttributeValue() { DataType = "String", StringValue = nameof(CustomerCreated) },

        }
    }
};

var response = client.SendMessageAsync(sendMessageRequest);

Console.WriteLine(response);
