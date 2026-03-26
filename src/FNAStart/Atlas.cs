using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace FNAStart
{
    public class Atlas : IDisposable
    {
        private readonly ContentManager contentManager;

        private List<Texture2D> textureList = [];
        private bool disposedValue;

        public Atlas(ContentManager contentManager)
        {
            this.contentManager = contentManager;
        }

        public int Count => textureList.Count;

        public void Load(string pathPrefix, int num)
        {
            for (int i = 0; i < num; i++)
            {
                var texture = contentManager.Load<Texture2D>(pathPrefix + (i + 1));
                textureList.Add(texture);
            }
        }

        public Texture2D? GetTexture(int index)
        {
            if (index < 0 || index >= textureList.Count)
            {
                return null;
            }
            return textureList[index];
        }

        public void AddTexture(Texture2D texture)
        {
            textureList.Add(texture);
        }

        public void Clear()
        {
            textureList.Clear();
        }

        #region IDisposable
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    foreach (var tex in textureList)
                    {
                        tex.Dispose();
                    }
                }

                disposedValue = true;
            }
        }

        ~Atlas()
        {
            Dispose(disposing: false);
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
