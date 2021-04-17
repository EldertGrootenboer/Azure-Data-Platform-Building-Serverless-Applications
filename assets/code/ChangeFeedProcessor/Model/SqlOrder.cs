using System;

public class SqlOrder
{
    public Guid id { get; set; }
    public string orderNumber { get; set; }
    public string orderStatus { get; set; }
    public int hubId { get; set; }
    public string customerName { get; set; }
    public string street { get; set; }
    public string city { get; set; }
    public string zipCode { get; set; }
    public int articleCount { get; set; }
}