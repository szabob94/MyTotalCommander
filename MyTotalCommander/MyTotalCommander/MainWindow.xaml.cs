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
using Microsoft.VisualBasic.FileIO;

namespace WpfApplication37
{
    public delegate void CopyFileOrDirectory(string honnan, string hova, UIOption option);
    public delegate void RefreshUI();
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
                DirectoryEntry d = new DirectoryEntry(s, s, "<Driver>", "<DIR>", Directory.GetLastWriteTime(s), null, EntryType.Dir);
                d.Imagepath = (ImageSource)new ImageSourceConverter().ConvertFromString("Images/dir.gif");
                entries.Add(d);
            }
            this.listView1.DataContext = entries;
            this.listView2.DataContext = entries;
            
        }

        void populateStayedListViews()
        {
            if (bal_listview_path != null && !bal_listview_path.Equals(""))
            {
                DirectoryInfo bal_dir = new DirectoryInfo(bal_listview_path);
                DirectoryEntry bal_d = new DirectoryEntry(
                        bal_dir.Name, bal_dir.FullName, "<Folder>", "<DIR>",
                        Directory.GetLastWriteTime(bal_listview_path),
                        null, EntryType.Dir);
                bal_d.Imagepath = (ImageSource)new ImageSourceConverter().ConvertFromString("Images/dir.gif");
                try {                    
                    showFilesAndDirectories(bal_d, bal_dir.Parent.FullName, subEntries);
                } catch(NullReferenceException e)
                {
                    showFilesAndDirectories(bal_d, null, subEntries);
                }
            }
            if (jobb_listview_path != null && !jobb_listview_path.Equals(""))
            {
                DirectoryInfo jobb_dir = new DirectoryInfo(jobb_listview_path);
                DirectoryEntry jobb_d = new DirectoryEntry(
                            jobb_dir.Name, jobb_dir.FullName, "<Folder>", "<DIR>",
                            Directory.GetLastWriteTime(jobb_listview_path),
                            null, EntryType.Dir);
                jobb_d.Imagepath = (ImageSource)new ImageSourceConverter().ConvertFromString("Images/dir.gif");                
                try
                {
                    showFilesAndDirectories(jobb_d, jobb_dir.Parent.FullName, subEntries2);
                }
                catch (NullReferenceException e)
                {
                    showFilesAndDirectories(jobb_d, null, subEntries2);
                }
            }
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
                    entry.Imagepath = (ImageSource)new ImageSourceConverter().ConvertFromString("Images/dir.gif");
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
                        DirectoryEntry d = new DirectoryEntry(s, s, "<Driver>", "<DIR>", Directory.GetLastWriteTime(s), null, EntryType.Dir);
                        d.Imagepath = (ImageSource)new ImageSourceConverter().ConvertFromString("Images/dir.gif");
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
                    entry.Imagepath = (ImageSource)new ImageSourceConverter().ConvertFromString("Images/dir.gif");
                    showFilesAndDirectories(entry, path, subEntries2);
                }
                else
                {
                    jobb_listview_path = null;
                    selected_item = null;
                    subEntries2.Clear();
                    foreach (string s in Directory.GetLogicalDrives())
                    {
                        DirectoryEntry d = new DirectoryEntry(s, s, "<Driver>", "<DIR>", Directory.GetLastWriteTime(s), null, EntryType.Dir);
                        d.Imagepath = (ImageSource)new ImageSourceConverter().ConvertFromString("Images/dir.gif");
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
                            null, EntryType.Dir);
            dd.Lastpath = path;
            dd.Imagepath = (ImageSource)new ImageSourceConverter().ConvertFromString("Images/dir.gif");
            subEntries.Add(dd);
            try
            {
                foreach (string s in Directory.GetDirectories(entry.Fullpath))
                {
                    DirectoryInfo dir = new DirectoryInfo(s);
                    DirectoryEntry d = new DirectoryEntry(
                        dir.Name, dir.FullName, "<Folder>", "<DIR>",
                        Directory.GetLastWriteTime(s),
                        null, EntryType.Dir);
                    d.Lastpath = entry.Fullpath;
                    d.Imagepath = (ImageSource)new ImageSourceConverter().ConvertFromString("Images/dir.gif");
                    subEntries.Add(d);
                }
                foreach (string f in Directory.GetFiles(entry.Fullpath))
                {
                    FileInfo file = new FileInfo(f);
                    DirectoryEntry d = new DirectoryEntry(
                        file.Name, file.FullName, file.Extension, file.Length.ToString(),
                        file.LastWriteTime,
                        null, EntryType.File);
                    subEntries.Add(d);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Nincs jogosultsága!", "Hiba", MessageBoxButton.OK);
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
            try {
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
                    populateStayedListViews();
                }
                else
                {
                    MessageBox.Show("Ilyen nevű mappa már létezik", "Hiba", MessageBoxButton.OK);
                }
            } catch(ArgumentException e)
            {
                MessageBox.Show("Itt nem hozhatsz létre mappát!", "Hiba", MessageBoxButton.OK);
            }           
        }

        private void ujmappa_click(object sender, RoutedEventArgs e)
        {
            string ujmappa_nev = Microsoft.VisualBasic.Interaction.InputBox("Adja meg az új mappa nevét", "Új mappa létrehozása", "Új mappa", -1, -1);
            
            
            if (selected_listview == 1 && bal_listview_path != null && !ujmappa_nev.Equals("") && !bal_listview_path.Equals("") && !ContainsAny(ujmappa_nev,"/",@"\","*",":","?","\"","<",">","|"))
            {
                CreateDirectory(ujmappa_nev, bal_listview_path);
            }
            else if (selected_listview == 2 && jobb_listview_path != null && !ujmappa_nev.Equals("") && !jobb_listview_path.Equals("") && !ContainsAny(ujmappa_nev, "/", @"\", "*", ":", "?", "\"", "<", ">", "|"))
            {
                CreateDirectory(ujmappa_nev, jobb_listview_path);
            }
            else
            {
                if (ujmappa_nev.Equals(""))
                    MessageBox.Show("Megszakítva!", "Hiba", MessageBoxButton.OK);
                else if(ContainsAny(ujmappa_nev, "/", @"\", "*", ":", "?", "\"", "<", ">", "|"))
                    MessageBox.Show("A mappa neve nem megengedett karaktert tartalmaz!", "Hiba", MessageBoxButton.OK);
                else
                    MessageBox.Show("Itt nem hozhatsz létre mappát!", "Hiba", MessageBoxButton.OK);
            }
        }

        private void DeleteDirectory(object sender, RoutedEventArgs e)
        {
            if (selected_item != null && selected_item.Fullpath.Length == 3)
            {
                MessageBox.Show("Meghajtót nem törölhetsz!", "Hiba", MessageBoxButton.OK);
            }
            else if (selected_item != null && selected_item.Name.Equals("...."))
            {
                MessageBox.Show("Nincs létező mappa kijelölve!", "Hiba", MessageBoxButton.OK);
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
            populateStayedListViews();
        }

        public void TaskCompleted(IAsyncResult R)
        {
            AsyncTransfer transfer = (AsyncTransfer)R.AsyncState;
            try {
                transfer.Delegatee.EndInvoke(R);
                RefreshUI refreshUI = new RefreshUI(populateStayedListViews);
                Dispatcher.BeginInvoke(refreshUI);
                if(transfer.Mode == 0)
                    MessageBox.Show(transfer.Allomany + " másolása megtörtént.", "Kész", MessageBoxButton.OK);
                else
                    MessageBox.Show(transfer.Allomany + " áthelyezése megtörtént.", "Kész", MessageBoxButton.OK);
            } catch (Exception e)
            {
                if (e is OperationCanceledException)
                {
                    if (transfer.Mode == 0)
                        MessageBox.Show(transfer.Allomany + " másolása megszakítva.", "Hiba", MessageBoxButton.OK);
                    else
                        MessageBox.Show(transfer.Allomany + " áthelyezése megszakítva.", "Hiba", MessageBoxButton.OK);
                } else if (e is IOException)
                {
                    MessageBox.Show(transfer.Allomany + " másolása nem hajtható végre!", "Hiba", MessageBoxButton.OK);
                }
            }
        }

        private void CopyOnClick(object sender, RoutedEventArgs e)
        {
            string hova;
            CopyFileOrDirectory delegatee=null;
            if (selected_listview == 1)
                hova = jobb_listview_path;
            else
                hova = bal_listview_path;
            
            if (selected_item.Type == EntryType.Dir)
            {
                delegatee = new CopyFileOrDirectory(FileSystem.CopyDirectory);
                delegatee.BeginInvoke(selected_item.Fullpath, hova + @"\" + selected_item.Name,
                UIOption.AllDialogs, new AsyncCallback(TaskCompleted), new AsyncTransfer(delegatee,selected_item.Name,0));
            } else if(selected_item.Type == EntryType.File)
            {
                delegatee = new CopyFileOrDirectory(FileSystem.CopyFile);
                delegatee.BeginInvoke(selected_item.Fullpath, hova + @"\" + selected_item.Name,
                UIOption.AllDialogs, new AsyncCallback(TaskCompleted), new AsyncTransfer(delegatee, selected_item.Name,0));
            } else
            {
                MessageBox.Show("A kijelölt elem nem másolható!", "Hiba", MessageBoxButton.OK);
            }
            populateStayedListViews();
        }

        private void MoveOnClick(object sender, RoutedEventArgs e)
        {
            string hova;
            CopyFileOrDirectory delegatee = null;
            if (selected_listview == 1)
                hova = jobb_listview_path;
            else
                hova = bal_listview_path;

            if (selected_item.Type == EntryType.Dir)
            {
                delegatee = new CopyFileOrDirectory(FileSystem.MoveDirectory);
                delegatee.BeginInvoke(selected_item.Fullpath, hova + @"\" + selected_item.Name,
                UIOption.AllDialogs, new AsyncCallback(TaskCompleted), new AsyncTransfer(delegatee, selected_item.Name, 1));
            }
            else if (selected_item.Type == EntryType.File)
            {
                delegatee = new CopyFileOrDirectory(FileSystem.MoveFile);
                delegatee.BeginInvoke(selected_item.Fullpath, hova + @"\" + selected_item.Name,
                UIOption.AllDialogs, new AsyncCallback(TaskCompleted), new AsyncTransfer(delegatee, selected_item.Name, 1));
            }
            else
            {
                MessageBox.Show("A kijelölt elem nem másolható!", "Hiba", MessageBoxButton.OK);
            }
            populateStayedListViews();
        }

        public bool ContainsAny(string haystack, params string[] needles)
        {
            foreach (string needle in needles)
            {
                if (haystack.Contains(needle))
                    return true;
            }

            return false;
        }
    }
   

    public enum EntryType
    {
        Dir,
        File
    }

    public class AsyncTransfer
    {
        public CopyFileOrDirectory _delegatee;
        public string _allomany;
        public int _mode;

        public AsyncTransfer(CopyFileOrDirectory delegatee, string allomany, int mode)
        {
            _delegatee = delegatee;
            _allomany = allomany;
            _mode = mode;
        }

        public CopyFileOrDirectory Delegatee
        {
            get { return _delegatee; }
            set { _delegatee = value; }
        }

        public string Allomany
        {
            get { return _allomany; }
            set { _allomany = value; }
        }

        public int Mode
        {
            get { return _mode; }
            set { _mode = value; }
        }
    }

    public class DirectoryEntry
    {
        private string _name;
        private string _fullpath;
        private string _lastpath;
        private string _ext;
        private string _size;
        private DateTime _date;
        private ImageSource _imagepath;
        private EntryType _type;

        public DirectoryEntry(string name,string fullname, string ext, string size, DateTime date, ImageSource imagepath, EntryType type)
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
        
        public ImageSource Imagepath
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
