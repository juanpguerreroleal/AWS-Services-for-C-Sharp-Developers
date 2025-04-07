
using System.Text.Json;
using Amazon.SQS;
using Amazon.SQS.Model;

CancellationTokenSource cts = new CancellationTokenSource();
AmazonSQSClient client = new AmazonSQSClient();

var queueUrlResponse = await client.GetQueueUrlAsync("customers");

ReceiveMessageRequest request = new ReceiveMessageRequest
{
    QueueUrl = queueUrlResponse.QueueUrl,
    MessageAttributeNames = new List<string>()
    {
        "MessageType"
    }
};

while (!cts.IsCancellationRequested)
{
    ReceiveMessageResponse response = await client.ReceiveMessageAsync(request, cts.Token);

    foreach (var message in response.Messages)
    {
        Console.WriteLine($"Message Id: {message.MessageId}");
        Console.WriteLine($"Message Body: {message.Body}");

        await client.DeleteMessageAsync(request.QueueUrl, message.ReceiptHandle);
    }

    await Task.Delay(3000);
}