using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Norm;
using Norm.Linq;
using Norm.Collections;
using System.IO;
using MongoDB.Driver;
using MongoDB.Driver.GridFS;
namespace SCORM_Repository.Models
{
    public class SCORMObjectModel : IDisposable
    {

        private IMongo _Connection;
        private IMongoCollection<SCORMObject> _DB;
        private Norm.GridFS.GridFileCollection _FileStore;
        private string _ConnectionString;
        public SCORMObjectModel(String connectionString)
        {
            _ConnectionString = connectionString;
            _Connection = Mongo.Create(connectionString);
            _DB = _Connection.GetCollection<SCORMObject>();
            _FileStore = Norm.GridFS.Helpers.Files<SCORMObject>(_DB);
        }
        public string GetMimeType(string fileName)
        {
            string mimeType = "application/unknown";
            string ext = System.IO.Path.GetExtension(fileName).ToLower();
            Microsoft.Win32.RegistryKey regKey = Microsoft.Win32.Registry.ClassesRoot.OpenSubKey(ext);
            if (regKey != null && regKey.GetValue("Content Type") != null)
                mimeType = regKey.GetValue("Content Type").ToString();
            return mimeType;
        }
        private IQueryable<SCORMObject> QueryInterface
        {
            get { return _DB.AsQueryable(); }
        }
        public SCORMObject Get(ObjectId id)
        {
            return (from item in QueryInterface
                    where item.ID == id
                    select item).First();
        }
        public SCORMObject Get(String id)
        {
            return Get(new ObjectId(id));
        }
        public void Delete(String id)
        {
            Delete(new ObjectId(id));
        }
        public void Delete(ObjectId id)
        {
            Delete(new SCORMObject { ID = id });
        }
        public void Delete(SCORMObject id)
        {
            _DB.Delete(id);
        }
        public IEnumerable<String> GetCategories()
        {
            return (from item in QueryInterface
                    select item.Category).ToList().Distinct();

        }
        public IEnumerable<SCORMObject> Search(string searchTerm)
        {
            searchTerm = searchTerm.ToLower();
            return from item in QueryInterface
                   where item.Title.ToLower().Contains(searchTerm) ||
                         item.Description.ToLower().Contains(searchTerm) ||
                         item.Category.ToLower().Contains(searchTerm)
                   select item;

        }
        public IEnumerable<SCORMObject> GetAll()
        {
            return QueryInterface.ToArray();
        }
        public IEnumerable<SCORMObject> GetByCategory(string category, int count = 0)
        {
            if (count > 0)
            {
                return (from obj in QueryInterface
                        where obj.Category.ToLower() == category.ToLower()
                        select obj).Take(count);
            }
            else
            {
                return from obj in QueryInterface
                       where obj.Category.ToLower() == category.ToLower()
                       select obj;
            }
        }
        public void Save(SCORMObject obj)
        {
            obj.Category = obj.Category.ToLower();
            _DB.Save(obj);            
            _DB.CreateIndex(y => new { y.Description, y.Title, y.Category }, "objectIndex", true, Norm.Protocol.Messages.IndexOption.Ascending);
        }
        public void AttachFile(Stream data, FileReference fileData, SCORMObject obj)
        {
            var rawData = new byte[data.Length];
            data.Read(rawData, 0, rawData.Length);
            AttachFile(rawData, fileData, obj);
        }
        public void AttachFile(byte[] data, FileReference fileData, SCORMObject obj)
        {
            List<FileReference> files = new List<FileReference>();
            if (obj.Files != null)
            {
                files.AddRange(obj.Files);
            }

            var db = MongoDB.Driver.MongoDatabase.Create(_ConnectionString);
            var gfs = new MongoGridFS(db);
            var id = Guid.NewGuid().ToString();
            using (var file = gfs.OpenWrite(id))
            {
                file.Write(data, 0, data.Length);
                file.Flush();
            }
            var fileInfo = gfs.FindOne(id);
            gfs.SetContentType(fileInfo, fileData.ContentType);
            fileData.FileId = id;
            files.Add(fileData);
            obj.Files = files;


        }
        public void DeleteFile(FileReference fileData)
        {
            _FileStore.Delete(new ObjectId(fileData.FileId));
        }
        public void Dispose()
        {
            _Connection.Dispose();
        }
        public ReturnFileStream GetFileStream(String objectId)
        {
            var db = MongoDB.Driver.MongoDatabase.Create(_ConnectionString);
            var gfs = new MongoGridFS(db);
            var value = gfs.FindOne(objectId);

            return new ReturnFileStream { Content = value.OpenRead(), ContentType = value.ContentType };
        }
        public ReturnFileStream GetFileStream(ObjectId objectId)
        {
            return GetFileStream(objectId.ToString());
        }
        public ReturnFileBytes GetFileBytes(string objectId)
        {
            var result = GetFileStream(objectId);
            byte[] data = new byte[result.Content.Length];
            using (Stream s = result.Content)
            {
                s.Read(data, 0, data.Length);
            }
            return new ReturnFileBytes { Content = data, ContentType = result.ContentType };

        }
        public ReturnFileBytes GetFileBytes(ObjectId objectId)
        {
            return GetFileBytes(objectId.ToString());
        }
    }
}