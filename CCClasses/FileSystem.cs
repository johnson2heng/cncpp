﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using CCClasses.FileFormats.Binary;

namespace CCClasses {
    public class FileSystem {
        public static String MainDir = "";

        private static Dictionary<String, CCFileClass> LoadedFiles = new Dictionary<string, CCFileClass>();

        public static CCFileClass LoadFile(String filename) {
            if (LoadedFiles.ContainsKey(filename)) {
                return LoadedFiles[filename];
            }
            if (filename.IndexOf(Path.DirectorySeparatorChar) != -1) {
                if (File.Exists(filename)) {
                    var ccf = new CCFileClass(filename, File.OpenRead(filename));
                    LoadedFiles.Add(filename, ccf);
                    return ccf;
                }
                return null;
            }

            foreach (var M in MIX.LoadedMIXes) {
                if (M.ContainsFile(filename)) {
                    var ccf = new CCFileClass(filename, M.GetFileContents(filename));
                    LoadedFiles.Add(filename, ccf);
                    return ccf;
                }
            }

            var loose = MainDir + filename;
            if (File.Exists(loose)) {
                var ccf = new CCFileClass(filename, File.OpenRead(loose));
                LoadedFiles.Add(filename, ccf);
                return ccf;
            }

            return null;
        }

        public static MIX LoadMIX(String filename) {
            var ccF = LoadFile(filename);
            if (ccF != null) {
                var MX = new MIX(ccF);
                MIX.LoadedMIXes.Insert(0, MX);
                return MX;
            }
            return null;
        }
    }
}
