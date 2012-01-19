using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using SCORM_Repository.Models;
using System.IO;
using System.Security.Cryptography;

namespace SCORM_Repository.Tests
{
    [TestFixture]
    public class SCORMObjectModelTest
    {

        public SCORMObjectModel Setup()
        {
            return new Models.SCORMObjectModel("mongodb://localhost:27018/tests");
        }
        [Test]
        public void TestSaveNew()
        {
            using (var model = Setup())
            {
                var testData = new Models.SCORMObject
                {
                    Title = "test",
                    Description = "test",
                    Category = "Test"
                };
                model.Save(testData);
                Assert.IsNotNull(testData.ID);
            }
        }
        [Test]
        public void TestSaveUpdate()
        {
            using (var model = Setup())
            {
                var content = model.GetAll();
                var obj = content.First();
                var origTitle = obj.Title;
                obj.Title = "blah";
                model.Save(obj);
                obj = model.Get(obj.ID);
                Assert.AreNotEqual(origTitle, obj.Title);
            }
        }
        [Test]
        public void TestGetByCategory()
        {
            using (var model = Setup())
            {
                var results = model.GetByCategory("Test");
                Assert.IsNotNull(results);
                Assert.IsTrue(results.Count() > 0);
            }
        }
        [Test]
        public void TestGetByCategoryFail()
        {
            using (var model = Setup())
            {
                var results = model.GetByCategory("1");
                Assert.IsNotNull(results);
                Assert.IsTrue(results.Count() == 0);
            }
        }
        [Test]
        public void TestGetAll()
        {
            using (var model = Setup())
            {
                var results = model.GetAll();
                Assert.IsNotNull(results);
                Assert.IsTrue(results.Count() > 0);
            }
        }
        [Test]
        public void TestAttachFile()
        {
            using (var model = Setup())
            {
                var contentObjects = model.GetAll().First();
                string fileName = "test.txt";
                byte[] data;
                using (MemoryStream stream = new MemoryStream())
                using (StreamWriter writer = new StreamWriter(stream))
                {

                    writer.Write("test file");
                    writer.Flush();
                    stream.Seek(0, SeekOrigin.Begin);
                    data = new byte[stream.Length];
                    stream.Read(data, 0, data.Length);
                }
                FileReference file = new FileReference();
                file.Filename = fileName;
                file.ContentType = model.GetMimeType(fileName);
                model.AttachFile(data, file, contentObjects);
                var savedData = model.GetFileBytes(file.FileId);
                var origHash = CalculateSHA1(data);
                var newHash = CalculateSHA1(savedData.Content);
                Assert.AreEqual(origHash, newHash);

            }
        }
        public string CalculateSHA1(byte[] buffer)
        {
            SHA1CryptoServiceProvider cryptoTransformSHA1 =
            new SHA1CryptoServiceProvider();
            string hash = BitConverter.ToString(
                cryptoTransformSHA1.ComputeHash(buffer)).Replace("-", "");

            return hash;
        }
    }
}
