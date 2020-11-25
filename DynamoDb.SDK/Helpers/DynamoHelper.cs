using System;
using System.IO;
using Amazon;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.Extensions.NETCore.Setup;
using Amazon.Runtime;
using DynamoDb.SDK.Configurations;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace DynamoDb.SDK.Helpers
{
    /// <summary>
    /// 
    /// </summary>
    public static class DynamoHelper
    {
        public static readonly JsonSerializerSettings JsonSerializerSettings = new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.Objects,
            ContractResolver = new CamelCasePropertyNamesContractResolver(),
            NullValueHandling = NullValueHandling.Ignore,
            Formatting = Formatting.None,
            Converters = new[] { new StringEnumConverter() },
            MetadataPropertyHandling = MetadataPropertyHandling.ReadAhead,
        };

        public static bool IsNumericType<TKey>()
        {
            switch (Type.GetTypeCode(typeof(TKey)))
            {
                case TypeCode.Byte:
                case TypeCode.SByte:
                case TypeCode.UInt16:
                case TypeCode.UInt32:
                case TypeCode.UInt64:
                case TypeCode.Int16:
                case TypeCode.Int32:
                case TypeCode.Int64:
                case TypeCode.Decimal:
                case TypeCode.Double:
                case TypeCode.Single:
                    return true;

                default:
                    return false;
            }
        }

        public static T Convert<T>(DynamoDBEntry entry)
        {
            Type type = typeof(T);
            if (type == typeof(bool)) return (T)(object)entry.AsBoolean();
            if (type == typeof(byte)) return (T)(object)entry.AsByte();
            if (type == typeof(byte[])) return (T)(object)entry.AsByteArray();
            if (type == typeof(char)) return (T)(object)entry.AsChar();
            if (type == typeof(DateTime)) return (T)(object)entry.AsDateTime();
            if (type == typeof(decimal)) return (T)(object)entry.AsDecimal();
            if (type == typeof(double)) return (T)(object)entry.AsDouble();
            if (type == typeof(Guid)) return (T)(object)entry.AsGuid();
            if (type == typeof(int)) return (T)(object)entry.AsInt();
            if (type == typeof(long)) return (T)(object)entry.AsLong();
            if (type == typeof(MemoryStream)) return (T)(object)entry.AsMemoryStream();
            if (type == typeof(sbyte)) return (T)(object)entry.AsSByte();
            if (type == typeof(short)) return (T)(object)entry.AsShort();
            if (type == typeof(float)) return (T)(object)entry.AsSingle();
            if (type == typeof(string)) return (T)(object)entry.AsString();
            if (type == typeof(uint)) return (T)(object)entry.AsUInt();
            if (type == typeof(ulong)) return (T)(object)entry.AsULong();
            if (type == typeof(ushort)) return (T)(object)entry.AsUShort();
            throw new InvalidOperationException($"{type.FullName} is not supported as aggregate key in DynamoDB");
        }

        //        public static Guid AsGuid(this ObjectId oid)
        //        {
        //            var bytes = oid.ToByteArray().Concat(new byte[] { 5, 5, 5, 5 }).ToArray();
        //            Guid gid = new Guid(bytes);
        //            return gid;
        //        }
        //
        //        /// <summary>
        //        /// Only Use to convert a Guid that was once an ObjectId
        //        /// </summary>
        //        public static ObjectId AsObjectId(this Guid gid)
        //        {
        //            var bytes = gid.ToByteArray().Take(12).ToArray();
        //            var oid = new ObjectId(bytes);
        //            return oid;
        //        }
        //
        //        /// <summary>
        //        /// Only Use to convert a Guid that was once an ObjectId
        //        /// </summary>
        //        public static ObjectId AsObjectId(this string id)
        //        {
        //            return MongoDB.Bson.ObjectId.Parse(id);
        //        }

        public static AWSOptions GetAwsOptions(IConfiguration configuration)
        {
            var awsConfig = new AWSConfig();
            configuration.GetSection("AWS").Bind(awsConfig);
            var awsOptions = new AWSOptions
            {
                Profile = awsConfig.Profile,
                Region = RegionEndpoint.GetBySystemName(awsConfig.Region),
                Credentials = new BasicAWSCredentials(awsConfig.AccessKey, awsConfig.SecretKey),
            };
            return awsOptions;
        }
    }
}