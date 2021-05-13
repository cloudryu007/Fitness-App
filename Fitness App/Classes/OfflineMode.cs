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

        public void offline()
        {
            Common.offline = true;
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
                    writer.WriteElementString("iduser", Common.iduser);
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
                XmlNodeList nodes = doc.DocumentElement.SelectNodes("User");
                XmlElement root = doc.DocumentElement;

                try
                {
                    string offline = root.GetAttribute("offlinemode");
                    if (offline != null && offline == "True")
                    {
                        foreach (XmlNode node in nodes)
                        {
                            switch (node.OuterXml)
                            {
                                case "iduser":
                                    Common.iduser = node.InnerText;
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
                            nodes = doc.DocumentElement.SelectNodes("Routine");
                            foreach (XmlNode node in nodes)
                            {
                                bool add = false;
                                string routineName = null;
                                string exerciseName = null;

                                foreach (XmlNode child in node.ChildNodes)
                                {
                                    if (child.Name == "RoutineName")
                                    {
                                        AddRemoveRoutine addRt = new AddRemoveRoutine();
                                        add = addRt.AddRoutine(child.InnerText);
                                        routineName = child.InnerText;
                                    }

                                    else
                                    {
                                        foreach (XmlNode sub in child.ChildNodes)
                                        {
                                            switch (sub.Name)
                                            {
                                                case "ExerciseName":
                                                    AddRemoveexercise addEx = new AddRemoveexercise();
                                                    add = addEx.Addexercise(sub.InnerText, routineName);
                                                    exerciseName = sub.InnerText;
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

                                                default:
                                                    continue;
                                            }
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

        private string getValue(List<int> list)
        {
            return string.Join(",", list.Select(x => x.ToString()).ToArray());
        }
    }
}
