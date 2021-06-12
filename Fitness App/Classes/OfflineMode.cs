using Fitness_App.Screens;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Fitness_App.Classes
{
    //incase we cannot connect to save users data online
    //create local XML to save the info, this should take priority upon logging in
    class OfflineMode
    {
        string dir = AppDomain.CurrentDomain.BaseDirectory + @"\OfflineData";
        string file = "profile.xml";
        string rsample = "Sample_Routine.xml";
        string wsample = "Sample_Workout.xml";
        string user = "user.xml";

        //local vars for parsing offline profile.xml
        string RoutineName = null;
        string ExerciseName = null;
        string SupersetName = null;

        //create offline data on users info
        public void offline()
        {
            if (!Directory.Exists(dir))
            {
                try
                {
                    Directory.CreateDirectory(dir);
                }

                catch (Exception ex)
                {
                    //something strange while attmepting to remove it
                    MsgBox msgbox = new MsgBox();
                    msgbox.MessageBox("Information", "Error while trying to create offline directory " + ex.Message, Types.OK, Icons.Information);
                    msgbox.ShowDialog();
                    return;
                }
            }

            if (Common.userWorkouts.Count > 0)
            {
                try
                {
                    //create local XML to upload later
                    XmlWriterSettings settings = new XmlWriterSettings();
                    settings.Indent = true;
                    settings.IndentChars = ("\t");
                    XmlWriter writer = XmlWriter.Create(dir + @"\" + file, settings);
                    writer.WriteStartElement("Routines");

                    //loop through the users app data
                    foreach (var routine in Common.userWorkouts)
                    {
                        writer.WriteStartElement("Routine");
                        writer.WriteElementString("RoutineName", routine.routine);

                        foreach (var exercise in routine.workout)
                        {
                            string reps = null; string weights = null;
                            writer.WriteStartElement("Exercise");
                            writer.WriteElementString("ExerciseName", exercise.exerciseName);
                            writer.WriteElementString("Sets", exercise.sets.ToString());
                            writer.WriteElementString("Reps", reps = getValue(exercise.reps));
                            writer.WriteElementString("Weight", weights = getValue(exercise.weight));

                            //look for superset related info
                            if(exercise.supersets.Count > 0)
                            {
                                foreach(var superset in exercise.supersets)
                                {
                                    reps = null; weights = null;
                                    writer.WriteStartElement("Superset");
                                    writer.WriteElementString("SupersetName", superset.supersetName);
                                    writer.WriteElementString("SupersetSets", superset.sets.ToString());
                                    writer.WriteElementString("SupersetReps", reps = getValue(superset.reps));
                                    writer.WriteElementString("SupersetWeight", weights = getValue(superset.weight));
                                    writer.WriteEndElement();
                                }
                            }

                            writer.WriteEndElement();
                        }

                        writer.WriteEndElement();
                    }

                    //clean up
                    writer.WriteEndElement();
                    writer.Flush();
                }

                catch (Exception ex)
                {
                    MsgBox msgbox = new MsgBox();
                    msgbox.MessageBox("Information", "Error while trying to save offline profile to drive... " + ex.Message, Types.OK, Icons.Information);
                    msgbox.ShowDialog();
                    return;
                }
            }

            if (Common.sampleRoutine.Count > 0)
            {
                try
                {
                    //create local XML to upload later
                    XmlWriterSettings settings = new XmlWriterSettings();
                    settings.Indent = true;
                    settings.IndentChars = ("\t");
                    XmlWriter writer = XmlWriter.Create(dir + @"\" + rsample, settings);
                    writer.WriteStartElement("Routines");

                    //loop through the users app data
                    foreach (var routine in Common.sampleRoutine)
                    {
                        writer.WriteElementString("RoutineName", routine);
                    }

                    //clean up
                    writer.WriteEndElement();
                    writer.Flush();
                }

                catch (Exception ex)
                {
                    MsgBox msgbox = new MsgBox();
                    msgbox.MessageBox("Information", "Error while trying to save offline routine sample to drive... " + ex.Message, Types.OK, Icons.Information);
                    msgbox.ShowDialog();
                    return;
                }
            }

            if (Common.sampleWorkout.Count > 0)
            {
                try
                {
                    //create local XML to upload later
                    XmlWriterSettings settings = new XmlWriterSettings();
                    settings.Indent = true;
                    settings.IndentChars = ("\t");
                    XmlWriter writer = XmlWriter.Create(dir + @"\" + wsample, settings);
                    writer.WriteStartElement("Workouts");

                    //loop through the users app data
                    foreach (var workout in Common.sampleWorkout)
                    {
                        writer.WriteStartElement("Workout");
                        writer.WriteElementString("Type", workout.Category);
                        writer.WriteElementString("Exercise", workout.Item);
                        writer.WriteEndElement();
                    }

                    //clean up
                    writer.WriteEndElement();
                    writer.Flush();
                }

                catch (Exception ex)
                {
                    MsgBox msgbox = new MsgBox();
                    msgbox.MessageBox("Information", "Error while trying to save offline workout sample to drive... " + ex.Message, Types.OK, Icons.Information);
                    msgbox.ShowDialog();
                    return;
                }
            }

            //save offline user profile
            if(Common.uid != null)
            {
                try
                {
                    //create local XML to upload later
                    XmlWriterSettings settings = new XmlWriterSettings();
                    settings.Indent = true;
                    settings.IndentChars = ("\t");
                    XmlWriter writer = XmlWriter.Create(dir + @"\" + user, settings);

                    writer.WriteStartElement("User");
                    writer.WriteAttributeString("offlinemode", Common.offline.ToString());//signify if we didn't actually update the DB
                    writer.WriteElementString("iduser", Common.userID);
                    writer.WriteElementString("firstName", Common.firstName);
                    writer.WriteElementString("lastName", Common.lastName);
                    writer.WriteElementString("city", Common.city);
                    writer.WriteElementString("state", Common.state);
                    writer.WriteElementString("zip", Common.zip);
                    writer.WriteElementString("phone", Common.phone);
                    writer.WriteEndElement();
                    writer.Flush();
                }

                catch (Exception ex)
                {
                    MsgBox msgbox = new MsgBox();
                    msgbox.MessageBox("Information", "Error while trying to save offline user to drive... " + ex.Message, Types.OK, Icons.Information);
                    msgbox.ShowDialog();
                    return;
                }
            }
        }

        //check if we have data in the XML file
        public bool getOfflineData()
        {
            if (File.Exists(dir + @"\" + user))
            {
                XmlDocument doc = new XmlDocument(); doc.Load(dir + @"\" + user);
                XmlElement root = doc.DocumentElement;

                try
                {
                    string offline = root.GetAttribute("offlinemode");
                    if (offline != null && offline == "True" || Common.offline)
                    {
                        foreach (XmlNode node in root)
                        {
                            switch (node.Name)
                            {
                                case "iduser":
                                    Common.userID = node.InnerText;
                                    break;
                                case "firstName":
                                    Common.firstName = node.InnerText;
                                    break;
                                case "lastName":
                                    Common.lastName = node.InnerText;
                                    break;
                                case "city":
                                    Common.city = node.InnerText;
                                    break;
                                case "state":
                                    Common.state = node.InnerText;
                                    break;
                                case "zip":
                                    Common.zip = node.InnerText;
                                    break;
                                case "phone":
                                    Common.phone = node.InnerText;
                                    break;
                                default:
                                    continue;
                            }
                        }

                        if (File.Exists(dir + @"\" + file))
                        {
                            doc = new XmlDocument(); doc.Load(dir + @"\" + file);
                            XmlNodeList nodes = doc.DocumentElement.SelectNodes("Routine");
                            foreach (XmlNode node in nodes)
                            {
                                bool add = false;
                                foreach (XmlNode child in node.ChildNodes)
                                {
                                    if (child.Name == "RoutineName")
                                    {
                                        AddRemoveRoutine addRt = new AddRemoveRoutine();
                                        add = addRt.AddRoutine(child.InnerText);
                                        RoutineName = child.InnerText;
                                    }

                                    else
                                    {
                                        foreach (XmlNode sub in child.ChildNodes)
                                        {
                                            add = processNode(sub, RoutineName, ExerciseName, SupersetName);
                                        }
                                    }
                                }

                                if (!add)
                                {
                                    MsgBox msgbox = new MsgBox();
                                    msgbox.MessageBox("Information", "Error while trying to read offline data from drive...", Types.OK, Icons.Information);
                                    msgbox.ShowDialog();
                                    return false;
                                }
                            }
                        }

                        if (Common.userWorkouts.Count > 0)
                        {
                            return true;
                        }

                        return false;
                    }

                    else
                    {
                        return false;
                    }
                }

                catch
                {
                    return false;
                }
            }

            else
            {
                return false;
            }
        }

        //process XML node into object array
        private bool processNode(XmlNode sub, string routineName, string exerciseName, string supersetName)
        {
            bool add = false;
            switch (sub.Name)
            {
                case "ExerciseName":
                    AddRemoveexercise addEx = new AddRemoveexercise();
                    add = addEx.Addexercise(sub.InnerText, routineName);
                    ExerciseName = sub.InnerText;
                    break;

                case "Sets":
                    AddRemoveexercise addSets = new AddRemoveexercise();
                    add = addSets.UpdateSet(int.Parse(sub.InnerText), routineName, exerciseName);
                    break;

                case "Reps":
                    List<string> reps = sub.InnerText.Split(',').ToList();
                    for (int i = 0; i < reps.Count; i++)
                    {
                        int r = int.Parse(reps[i]);
                        AddRemoveexercise addReps = new AddRemoveexercise();
                        add = addReps.UpdateReps(r, routineName, exerciseName, i);
                    }
                    break;

                case "Weight":
                    List<string> weights = sub.InnerText.Split(',').ToList();
                    for (int i = 0; i < weights.Count; i++)
                    {
                        int w = int.Parse(weights[i]);
                        AddRemoveexercise addWeight = new AddRemoveexercise();
                        add = addWeight.UpdateWeight(w, routineName, exerciseName, i);
                    }
                    break;

                case "Superset":
                    foreach (XmlNode ss in sub.ChildNodes)
                    {
                        //recursive call
                        add = processNode(ss, RoutineName, ExerciseName, SupersetName);
                    }
                    break;

                case "SupersetName":
                    AddRemoveexercise addS = new AddRemoveexercise();
                    add = addS.AddSuperset(routineName, exerciseName, sub.InnerText);
                    SupersetName = sub.InnerText;
                    break;

                case "SupersetSets":
                    AddRemoveexercise addSSets = new AddRemoveexercise();
                    add = addSSets.UpdateSuperset(routineName, exerciseName, supersetName, int.Parse(sub.InnerText));
                    break;

                case "SupersetReps":
                    List<string> ssReps = sub.InnerText.Split(',').ToList();
                    for (int i = 0; i < ssReps.Count; i++)
                    {
                        int r = int.Parse(ssReps[i]);
                        AddRemoveexercise addSSReps = new AddRemoveexercise();
                        add = addSSReps.UpdateSupersetRW(routineName, exerciseName, supersetName, r, 0, i);
                    }
                    break;

                case "SupersetWeight":
                    List<string> ssWeights = sub.InnerText.Split(',').ToList();
                    for (int i = 0; i < ssWeights.Count; i++)
                    {
                        int w = int.Parse(ssWeights[i]);
                        AddRemoveexercise addSSWeight = new AddRemoveexercise();
                        add = addSSWeight.UpdateSupersetRW(routineName, exerciseName, supersetName, 0, w, i);
                    }
                    break;
            }

            return add;
        }

        //see if we have offline samples to select from when in offline mode (this is normally taken from DB)
        public void getOfflineSample()
        {
            if (File.Exists(dir + @"\" + rsample))
            {
                XmlDocument doc = new XmlDocument(); doc.Load(dir + @"\" + rsample);
                XmlNodeList nodes = doc.DocumentElement.SelectNodes("Routines");
                foreach (XmlNode node in nodes)
                {
                    Common.sampleRoutine.Add(node.InnerText);
                }
            }

            if (File.Exists(dir + @"\" + wsample))
            {
                XmlDocument doc = new XmlDocument(); doc.Load(dir + @"\" + wsample);
                XmlNodeList nodes = doc.DocumentElement.SelectNodes("Workout");
                foreach (XmlNode node in nodes)
                {
                    string type = null; string exercise = null;
                    foreach (XmlNode child in node.ChildNodes)
                    {
                        if(child.OuterXml == "Type")
                        {
                            type = child.InnerText;
                        }

                        else if(child.OuterXml == "Exercise")
                        {
                            exercise = child.InnerText;
                        }

                        if(type != null && exercise != null)
                        {
                            Common.sampleWorkout.Add(new SampleWorkout() { Category = type, Item = exercise });
                        }
                    }
                }
            }
        }

        //return comma delmited string from array
        private string getValue(List<int> list)
        {
            return string.Join(",", list.Select(x => x.ToString()).ToArray());
        }
    }
}
