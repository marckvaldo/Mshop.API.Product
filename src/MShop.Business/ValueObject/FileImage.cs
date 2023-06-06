﻿namespace MShop.Business.ValueObject
{
    public class FileImage
    {
        public string Path { get; private set; }
        public FileImage(string? path)
        {
            Path = path;
        }
    }
}
