using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Collections.ObjectModel;
using System.IO;

namespace WpfApplication37
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        ObservableCollection<DirectoryEntry> entries = new ObservableCollection<DirectoryEntry>();
        ObservableCollection<DirectoryEntry> subEntries = new ObservableCollection<DirectoryEntry>();
        ObservableCollection<DirectoryEntry> subEntries2 = new ObservableCollection<DirectoryEntry>();

        String bal_listview_path = "";
        String jobb_listview_path = "";
        int selected_listview = -1;
        DirectoryEntry selected_item = null;

        void Window1_Loaded(object sender, RoutedEventArgs e)
        {
            foreach (string s in Directory.GetLogicalDrives())
            {
                DirectoryEntry d = new DirectoryEntry(s, s, "<Driver>", "<DIR>", Directory.GetLastWriteTime(s), "Images/dir.gif", EntryType.Dir);
                entries.Add(d);
            }
            this.listView1.DataContext = entries;
            this.listView2.DataContext = entries;
            
        }

        void listViewItem_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            ListViewItem item = e.Source as ListViewItem;

            DirectoryEntry entry = item.DataContext as DirectoryEntry;
            String path;


            if (entry.Type == EntryType.Dir)
            {
                if (entry.Fullpath != null)
                {
                    bal_listview_path = entry.Fullpath;
                    if (entry.Fullpath.Length == 3)
                    {
                        path = null;
                    } else if (entry.Fullpath.LastIndexOf(@"\") == 2)
                    {
                        path = entry.Fullpath.Substring(0, entry.Fullpath.LastIndexOf(@"\")+1);
                    } else
                    {
                        path = entry.Fullpath.Substring(0, entry.Fullpath.LastIndexOf(@"\"));
                    }

                    selected_item = null;
                    showFilesAndDirectories(entry, path, subEntries);
                }
                else
                {
                    bal_listview_path = null;
                    selected_item = null;
                    subEntries.Clear();
                    foreach (string s in Directory.GetLogicalDrives())
                    {
                        DirectoryEntry d = new DirectoryEntry(s, s, "<Driver>", "<DIR>", Directory.GetLastWriteTime(s), "Images/dir.gif", EntryType.Dir);
                        subEntries.Add(d);
                    }
                }
                listView1.DataContext = subEntries;
            } else
            {
                System.Diagnostics.Process.Start(@entry.Fullpath);
            }
        }

        void listViewItem2_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            ListViewItem item = e.Source as ListViewItem;

            DirectoryEntry entry = item.DataContext as DirectoryEntry;
            String path;


            if (entry.Type == EntryType.Dir)
            {
                if (entry.Fullpath != null)
                {
                    jobb_listview_path = entry.Fullpath;
                    if (entry.Fullpath.Length == 3)
                    {
                        path = null;
                    }
                    else if (entry.Fullpath.LastIndexOf(@"\") == 2)
                    {
                        path = entry.Fullpath.Substring(0, entry.Fullpath.LastIndexOf(@"\") + 1);
                    }
                    else
                    {
                        path = entry.Fullpath.Substring(0, entry.Fullpath.LastIndexOf(@"\"));
                    }

                    selected_item = null;
                    showFilesAndDirectories(entry, path, subEntries2);
                }
                else
                {
                    jobb_listview_path = null;
                    selected_item = null;
                    subEntries2.Clear();
                    foreach (string s in Directory.GetLogicalDrives())
                    {
                        DirectoryEntry d = new DirectoryEntry(s, s, "<Driver>", "<DIR>", Directory.GetLastWriteTime(s), "Images/dir.gif", EntryType.Dir);
                        subEntries2.Add(d);
                    }
                }
                listView2.DataContext = subEntries2;
            }
            else
            {
                System.Diagnostics.Process.Start(@entry.Fullpath);
            }
        }

        void showFilesAndDirectories(DirectoryEntry entry, String path, ObservableCollection<DirectoryEntry> subEntries)
        {
            subEntries.Clear();
            DirectoryEntry dd = new DirectoryEntry(
                            "....", path, "<Folder>", "<DIR>",
                            entry.Date,
                            "Images/folder.gif", EntryType.Dir);
            dd.Lastpath = path;
            subEntries.Add(dd);
            try
            {
                foreach (string s in Directory.GetDirectories(entry.Fullpath))
                {
                    DirectoryInfo dir = new DirectoryInfo(s);
                    DirectoryEntry d = new DirectoryEntry(
                        dir.Name, dir.FullName, "<Folder>", "<DIR>",
                        Directory.GetLastWriteTime(s),
                        "Images/folder.gif", EntryType.Dir);
                    d.Lastpath = entry.Fullpath;
                    subEntries.Add(d);
                }
                foreach (string f in Directory.GetFiles(entry.Fullpath))
                {
                    FileInfo file = new FileInfo(f);
                    DirectoryEntry d = new DirectoryEntry(
                        file.Name, file.FullName, file.Extension, file.Length.ToString(),
                        file.LastWriteTime,
                        "Images/file.gif", EntryType.File);
                    subEntries.Add(d);
                }
            }
            catch (IOException exception)
            {

            }
        }

        private void listViewItem_Click(object sender, RoutedEventArgs e)
        {
            ListViewItem item = e.Source as ListViewItem;
            DirectoryEntry entry = item.DataContext as DirectoryEntry;
            selected_listview = 1;
            selected_item = entry;
        }
        private void listViewItem2_Click(object sender, RoutedEventArgs e)
        {
            ListViewItem item = e.Source as ListViewItem;
            DirectoryEntry entry = item.DataContext as DirectoryEntry;
            selected_listview = 2;
            selected_item = entry;
        }

        private void CreateDirectory(string ujmappa_nev, string listview_path)
        {
            bool alreadyExists = false;
            foreach (string s in Directory.GetDirectories(listview_path))
            {
                DirectoryInfo directoryInfo = new DirectoryInfo(s);
                if (directoryInfo.Name.Equals(ujmappa_nev))
                {
                    alreadyExists = true;
                }
            }
            if (!alreadyExists)
            {
                Directory.CreateDirectory(listview_path + "\\" + ujmappa_nev);
                DirectoryInfo dir = new DirectoryInfo(listview_path);
                DirectoryEntry d = new DirectoryEntry(
                            dir.Name, dir.FullName, "<Folder>", "<DIR>",
                            Directory.GetLastWriteTime(listview_path),
                            "", EntryType.Dir);
                if (bal_listview_path.Equals(jobb_listview_path))
                {
                    showFilesAndDirectories(d, jobb_listview_path, subEntries);
                    showFilesAndDirectories(d, jobb_listview_path, subEntries2);
                } else if (selected_listview == 1)
                    showFilesAndDirectories(d, listview_path, subEntries);
                else
                    showFilesAndDirectories(d, listview_path, subEntries2);
            }
            else
            {
                MessageBox.Show("Ilyen nevű mappa már létezik", "Hiba", MessageBoxButton.OK);
            }
        }

        private void ujmappa_click(object sender, RoutedEventArgs e)
        {
            string ujmappa_nev = Microsoft.VisualBasic.Interaction.InputBox("Adja meg az új mappa nevét", "Új mappa létrehozása", "Új mappa", -1, -1);
            
            
            if (selected_listview == 1 && bal_listview_path != null)
            {
                CreateDirectory(ujmappa_nev, bal_listview_path);
            }
            else if (selected_listview == 2 && jobb_listview_path != null)
            {
                CreateDirectory(ujmappa_nev, jobb_listview_path);
            }
            else
            {
                MessageBox.Show("Itt nem hozhatsz létre mappát!", "Hiba", MessageBoxButton.OK);
            }
        }

        private void DeleteDirectory(object sender, RoutedEventArgs e)
        {
            if (selected_item != null && selected_item.Fullpath.Length == 3)
            {
                MessageBox.Show("Meghajtót nem törölhetsz!", "Hiba", MessageBoxButton.OK);
            }
            else if (selected_item != null && selected_item.Type == EntryType.Dir)
            {
                var result = MessageBox.Show("Biztosan törölni szeretné a mappát?", "Mappa törlése", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    Directory.Delete(selected_item.Fullpath, true);
                    selected_item = null;
                }
            }
            else if(selected_item != null && selected_item.Type == EntryType.File)
            {
                var result = MessageBox.Show("Biztosan törölni szeretné a fájlt?", "Fájl törlése", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    File.Delete(selected_item.Fullpath);
                    selected_item = null;
                }
            } else
            {
                MessageBox.Show("Nincs kijelölve elem", "Hiba", MessageBoxButton.OK);
            }
            DirectoryInfo dir = new DirectoryInfo(bal_listview_path);
            DirectoryEntry d = new DirectoryEntry(
                        dir.Name, dir.FullName, "<Folder>", "<DIR>",
                        Directory.GetLastWriteTime(bal_listview_path),
                        "", EntryType.Dir);
            if (bal_listview_path.Equals(jobb_listview_path))
            {
                showFilesAndDirectories(d, jobb_listview_path, subEntries);
                showFilesAndDirectories(d, jobb_listview_path, subEntries2);
            }
            else if (selected_listview == 1)
                showFilesAndDirectories(d, bal_listview_path, subEntries);
            else
                showFilesAndDirectories(d, jobb_listview_path, subEntries2);
        }

    }

    public enum EntryType
    {
        Dir,
        File
    }

    public class DirectoryEntry
    {
        private string _name;
        private string _fullpath;
        private string _lastpath;
        private string _ext;
        private string _size;
        private DateTime _date;
        private string _imagepath;
        private EntryType _type;

        public DirectoryEntry(string name,string fullname, string ext, string size, DateTime date, string imagepath, EntryType type)
        {
            _name = name;
            _fullpath = fullname;
            _ext = ext;
            _size = size;
            _date = date;
            _imagepath = imagepath;
            _type = type;
        }

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }
        
        public string Ext
        {
            get { return _ext; }
            set { _ext = value; }
        }

        public string Size
        {
            get { return _size; }
            set { _size = value; }
        }
        
        public DateTime Date
        {
            get { return _date; }
            set { _date = value; }
        }
        
        public string Imagepath
        {
            get { return _imagepath; }
            set { _imagepath = value; }
        }

        public EntryType Type
        {
            get { return _type; }
            set { _type = value; }
        }

        public string Fullpath
        {
            get { return _fullpath; }
            set { _fullpath = value; }
        }

        public string Lastpath
        {
            get { return _lastpath; }
            set { _lastpath = value; }
        }
    }
  }
