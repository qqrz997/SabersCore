using System;
using System.IO;

namespace SabersCore.Models;

public record SaberFileInfo(
    FileInfo FileInfo,
    string Hash,
    DateTime DateAdded);
