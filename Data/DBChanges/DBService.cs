using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace SRWords
{
    public static class DBService
    {
        public static void MakeChangesFromServer(List<ChangeInfo> list, IRepository repository)
        {
            ProcessChanges(list, repository);
        }
        public static void MakeChangesFromServer()
        {
            List<ChangeInfo> list = new List<ChangeInfo>();
            /*
            ChangeInfo info;
            info = new ChangeInfo(48, "D", "potpretsednik", "заместитель председателя;вице-президент", ""); list.Add(info);
            info = new ChangeInfo(49, "I", "potpredsednik", "заместитель председателя;вице-президент", "по“тпре_дседни_к$C#м$#.#1$D#)#замести’тель$RV#председа’теля$RV#;#2$D#)#ви’це-президе’нт$MRV#.#"); list.Add(info);
            info = new ChangeInfo(50, "D", "potpretsednički", "относящийся к заместителю председателя;вице-президентский", "по“тпре_дседни_чк||и_$C#,#~а_$C#,#~о_$C#1$D#)#относя’щийся$RV#к$RV#замести’телю$RV#председа’теля$RV#;#~а$C#дужност$CL#обя’занности$#замести’теля$#председа’теля$#;#2$D#)#ви’це-президе’нтский$MRV#.#"); list.Add(info);
            info = new ChangeInfo(51, "I", "potpredsednički", "относящийся к заместителю председателя;вице-президентский", "по“тпре_дседни_чк||и_$C#,#~а_$C#,#~о_$C#1$D#)#относя’щийся$RV#к$RV#замести’телю$RV#председа’теля$RV#;#~а$C#дужност$CL#обя’занности$#замести’теля$#председа’теля$#;#2$D#)#ви’це-президе’нтский$MRV#.#"); list.Add(info);
            info = new ChangeInfo(52, "D", "pretsedavati", "председательствовать", "председа’вати$C#,#предсе‛да_ва_м$C#председа’тельствовать$RV#.#"); list.Add(info);
            info = new ChangeInfo(53, "I", "predsedavati", "председательствовать", "председа’вати$C#,#предсе‛да_ва_м$C#председа’тельствовать$RV#.#"); list.Add(info);
            info = new ChangeInfo(54, "D", "pretsednik", "председатель;президент", "пре’дседни_к$C#м$#.#председа’тель$RV#;#президе’нт$RV#;#~$C#Министарског$C#Савета$C#председа’тель$#сове’та$#мини’стров$#,#премье’р-$M#мини’стр$#;#~$C#државе$C#президе’нт$#(#какого-$M#л$#.#государства$#)#.#"); list.Add(info);
            info = new ChangeInfo(55, "I", "predsednik", "председатель;президент", "пре’дседни_к$C#м$#.#председа’тель$RV#;#президе’нт$RV#;#~$C#Министарског$C#Савета$C#председа’тель$#сове’та$#мини’стров$#,#премье’р-$M#мини’стр$#;#~$C#државе$C#президе’нт$#(#какого-$M#л$#.#государства$#)#.#"); list.Add(info);
            info = new ChangeInfo(56, "D", "pretsednički", "председательский;президентский", "пре’дседни_чк||и_$C#,#~а_$C#,#~о_$C#1$D#)#председа’тельский$RV#;#~о$C#место$CL#≈$S#председа’тельское$#кре’сло$#;#2$D#)#президе’нтский$RV#;#~и$C#избори$C#президе’нтские$#вы’боры$#,#вы’боры$#президе’нта$#.#"); list.Add(info);
            info = new ChangeInfo(57, "I", "predsednički", "председательский;президентский", "пре’дседни_чк||и_$C#,#~а_$C#,#~о_$C#1$D#)#председа’тельский$RV#;#~о$C#место$CL#≈$S#председа’тельское$#кре’сло$#;#2$D#)#президе’нтский$RV#;#~и$C#избори$C#президе’нтские$#вы’боры$#,#вы’боры$#президе’нта$#.#"); list.Add(info);
            info = new ChangeInfo(58, "D", "pretsedništvo", "президиум;президентство;председательствование", "пре’дседни_штво$C#с$#.#1$D#)#прези’диум$RV#;#2$D#)#президе’нтство$RV#;#3$D#)#председа’тельствование$RV#.#"); list.Add(info);
            info = new ChangeInfo(59, "I", "predsedništvo", "президиум;президентство;председательствование", "пре’дседни_штво$C#с$#.#1$D#)#прези’диум$RV#;#2$D#)#президе’нтство$RV#;#3$D#)#председа’тельствование$RV#.#"); list.Add(info);
            info = new ChangeInfo(60, "D", "pretskazati", "предсказать", "предска’зати$C#,#пре‛дска_же_м$C#(#несов$#.#предскази’вати$C#)#предсказа’ть$RV#.#"); list.Add(info);
            info = new ChangeInfo(61, "I", "predskazati", "предсказать", "предска’зати$C#,#пре‛дска_же_м$C#(#несов$#.#предскази’вати$C#)#предсказа’ть$RV#.#"); list.Add(info);
            info = new ChangeInfo(62, "D", "pretskazivati", "предсказывать;предвещать", "предскази’вати$C#,#предска‛зује_м$C#(#сов$#.#предска’зати$CL#)#предска’зывать$RV#;#предвеща’ть$RV#.#"); list.Add(info);
            info = new ChangeInfo(63, "I", "predskazivati", "предсказывать;предвещать", "предскази’вати$C#,#предска‛зује_м$C#(#сов$#.#предска’зати$CL#)#предска’зывать$RV#;#предвеща’ть$RV#.#"); list.Add(info);
            info = new ChangeInfo(64, "D", "pretslutnja", "предчувствие", "пре‛дслу_тња$C#ж$#.#предчу’вствие$RV#.#"); list.Add(info);
            info = new ChangeInfo(65, "I", "predslutnja", "предчувствие", "пре‛дслу_тња$C#ж$#.#предчу’вствие$RV#.#"); list.Add(info);
            info = new ChangeInfo(66, "D", "pretsmrtan", "предсмертный", "пре‛дсмрт||ан$C#,#~ни_$C#,#~на$C#,#~но$C#предсме’ртный$RV#;#◊$S#~ни$C#ропац$CL#аго’ния$#.#"); list.Add(info);
            info = new ChangeInfo(67, "I", "predsmrtan", "предсмертный", "пре‛дсмрт||ан$C#,#~ни_$C#,#~на$C#,#~но$C#предсме’ртный$RV#;#◊$S#~ни$C#ропац$CL#аго’ния$#.#"); list.Add(info);
            info = new ChangeInfo(68, "D", "pretsoblje", "передняя;прихожая", "пре‛дсо_бље$C#с$#.#пере’дняя$RV#,#прихо’жая$RV#.#"); list.Add(info);
            info = new ChangeInfo(69, "I", "predsoblje", "передняя;прихожая", "пре‛дсо_бље$C#с$#.#пере’дняя$RV#,#прихо’жая$RV#.#"); list.Add(info);
            info = new ChangeInfo(70, "D", "pretsrđe", "предсердие", "пре‛дср_ђе$C#с$#.#анат$#.#предсе’рдие$RV#.#"); list.Add(info);
            info = new ChangeInfo(71, "I", "predsrđe", "предсердие", "пре‛дср_ђе$C#с$#.#анат$#.#предсе’рдие$RV#.#"); list.Add(info);
            info = new ChangeInfo(72, "D", "pretstava", "представление;понятие;спектакль;киносеанс", "пре“дстава$C#ж$#.#1$D#)#представле’ние$RV#,#поня’тие$RV#;#2$D#)#спекта’кль$RV#;#киносеа’нс$RV#.#"); list.Add(info);
            info = new ChangeInfo(73, "I", "predstava", "представление;понятие;спектакль;киносеанс", "пре“дстава$C#ж$#.#1$D#)#представле’ние$RV#,#поня’тие$RV#;#2$D#)#спекта’кль$RV#;#киносеа’нс$RV#.#"); list.Add(info);
            info = new ChangeInfo(74, "D", "pretstaviti", "представить;вообразить;изобразить;сыграть;представиться;умереть;преставиться", "пре‛дставити$C#,#-ви_м$MC#(#несов$#.#пре‛дстављати$C#)#1$D#)#предста’вить$RV#,#вообрази’ть$RV#;#2$D#)#изобрази’ть$RV#,#сыгра’ть$RV#(#кого-$M#л$#.#)#;#~$C#се$CL#1$D#)#предста’виться$RV#;#2$D#)#умере’ть$RV#,#преста’виться$RV#.#"); list.Add(info);
            info = new ChangeInfo(75, "I", "predstaviti", "представить;вообразить;изобразить;сыграть;представиться;умереть;преставиться", "пре‛дставити$C#,#-ви_м$MC#(#несов$#.#пре‛дстављати$C#)#1$D#)#предста’вить$RV#,#вообрази’ть$RV#;#2$D#)#изобрази’ть$RV#,#сыгра’ть$RV#(#кого-$M#л$#.#)#;#~$C#се$CL#1$D#)#предста’виться$RV#;#2$D#)#умере’ть$RV#,#преста’виться$RV#.#"); list.Add(info);
            info = new ChangeInfo(76, "D", "pretstavka", "докладная записка;письменный доклад;предложение;представление", "пре‛дста_вка$C#ж$#.#докладна’я$RV#запи’ска$RV#,#пи’сьменный$RV#докла’д$RV#;#предложе’ние$RV#;#представле’ние$RV#.#"); list.Add(info);
            info = new ChangeInfo(77, "I", "predstavka", "докладная записка;письменный доклад;предложение;представление", "пре‛дста_вка$C#ж$#.#докладна’я$RV#запи’ска$RV#,#пи’сьменный$RV#докла’д$RV#;#предложе’ние$RV#;#представле’ние$RV#.#"); list.Add(info);
            info = new ChangeInfo(78, "D", "pretstavljati", "представлять;воображать;изображать;играть;означать;представлять собой;представляться", "пре‛дстављати$C#,#-ља_м$MC#(#сов$#.#пре‛дставити$CL#)#1$D#)#представля’ть$RV#,#вообража’ть$RV#;#2$D#)#изобража’ть$RV#,#игра’ть$RV#(#кого-$M#л$#.#)#;#3$D#)#означа’ть$RV#,#представля’ть$RV#собо’й$RV#;#~$C#се$CL#представля’ться$RV#.#"); list.Add(info);
            info = new ChangeInfo(79, "I", "predstavljati", "представлять;воображать;изображать;играть;означать;представлять собой;представляться", "пре‛дстављати$C#,#-ља_м$MC#(#сов$#.#пре‛дставити$CL#)#1$D#)#представля’ть$RV#,#вообража’ть$RV#;#2$D#)#изобража’ть$RV#,#игра’ть$RV#(#кого-$M#л$#.#)#;#3$D#)#означа’ть$RV#,#представля’ть$RV#собо’й$RV#;#~$C#се$CL#представля’ться$RV#.#"); list.Add(info);
            info = new ChangeInfo(80, "D", "pretstavnik", "представитель;уполномоченное лицо", "пре‛дста_вни_к$C#м$#.#представи’тель$RV#;#уполномо’ченное$RV#лицо’$RV#.#"); list.Add(info);
            info = new ChangeInfo(81, "I", "predstavnik", "представитель;уполномоченное лицо", "пре‛дста_вни_к$C#м$#.#представи’тель$RV#;#уполномо’ченное$RV#лицо’$RV#.#"); list.Add(info);
            info = new ChangeInfo(82, "D", "pretstavnički", "представительский;представительный", "пре‛дста_вни_чк||и_$C#,#~а_$C#,#~о$C#1$D#)#представи’тельский$RV#;#2$D#)#представи’тельный$RV#;#~о$C#тело$CL#представи’тельный$#о’рган$#;#вы’борный$#о’рган$#.#"); list.Add(info);
            info = new ChangeInfo(83, "I", "predstavnički", "представительский;представительный", "пре‛дста_вни_чк||и_$C#,#~а_$C#,#~о$C#1$D#)#представи’тельский$RV#;#2$D#)#представи’тельный$RV#;#~о$C#тело$CL#представи’тельный$#о’рган$#;#вы’борный$#о’рган$#.#"); list.Add(info);
            info = new ChangeInfo(84, "D", "pretstavništvo", "представительство", "пре‛дста_вни_штво$C#с$#.#представи’тельство$RV#;#политичко$C#~$C#полити’ческое$#представи’тельство$#,#полпре’дство$#;#трговачко$C#~$C#торго’вое$#представи’тельство$#,#торгпре’дство$#.#"); list.Add(info);
            info = new ChangeInfo(85, "I", "predstavništvo", "представительство", "пре‛дста_вни_штво$C#с$#.#представи’тельство$RV#;#политичко$C#~$C#полити’ческое$#представи’тельство$#,#полпре’дство$#;#трговачко$C#~$C#торго’вое$#представи’тельство$#,#торгпре’дство$#.#"); list.Add(info);
            info = new ChangeInfo(86, "D", "pretstajanje", "появление;явка", "пре‛дстаја_ње$C#с$#.#,#пре‛дст||анак$C#,#~а_нка$C#м$#.#появле’ние$RV#(#перед$#кем-$M#л$#.#)#;#я’вка$RV#.#"); list.Add(info);
            info = new ChangeInfo(87, "I", "predstajanje", "появление;явка", "пре‛дстаја_ње$C#с$#.#,#пре‛дст||анак$C#,#~а_нка$C#м$#.#появле’ние$RV#(#перед$#кем-$M#л$#.#)#;#я’вка$RV#.#"); list.Add(info);
            info = new ChangeInfo(88, "D", "pretstanak", "появление;явка", "пре‛дст||анак$C#,#~а_нка$C#м$#.#появле’ние$RV#(#перед$#кем-$M#л$#.#)#;#я’вка$RV#см$#.#предстајање$CL#.#"); list.Add(info);
            info = new ChangeInfo(89, "I", "predstanak", "появление;явка", "пре‛дст||анак$C#,#~а_нка$C#м$#.#появле’ние$RV#(#перед$#кем-$M#л$#.#)#;#я’вка$RV#см$#.#предстајање$CL#.#"); list.Add(info);
            info = new ChangeInfo(90, "D", "pretstajati", "появляться;являться", "пре‛дстајати$C#,#-је_м$MC#(#сов$#.#предстати$C#)#появля’ться$RV#,#явля’ться$RV#;#~$C#суду$C#представа’ть$#пе’ред$#судо’м$#.#"); list.Add(info);
            info = new ChangeInfo(91, "I", "predstajati", "появляться;являться", "пре‛дстајати$C#,#-је_м$MC#(#сов$#.#предстати$C#)#появля’ться$RV#,#явля’ться$RV#;#~$C#суду$C#представа’ть$#пе’ред$#судо’м$#.#"); list.Add(info);
            
            info = new ChangeInfo(92, "D", "pretstati", "появиться;явиться;предстать", "пре‛дстати$C#,#пре‛дстане_м$C#(#несов$#.#предстајати$CL#)#появи’ться$RV#,#яви’ться$RV#;#предста’ть$RV#(#перед$#кем-$M#л$#.#)#;#~$C#лично$C#($PC#главом$C#)$PC#яви’ться$#ли’чно$#.#"); list.Add(info);
            info = new ChangeInfo(93, "I", "predstati", "появиться;явиться;предстать", "пре‛дстати$C#,#пре‛дстане_м$C#(#несов$#.#предстајати$CL#)#появи’ться$RV#,#яви’ться$RV#;#предста’ть$RV#(#перед$#кем-$M#л$#.#)#;#~$C#лично$C#($PC#главом$C#)$PC#яви’ться$#ли’чно$#.#"); list.Add(info);
            
            info = new ChangeInfo(94, "D", "pretstojati", "предстоять", "предсто‛јати$C#,#-ји_$MC#предстоя’ть$RV#.#"); list.Add(info);
            info = new ChangeInfo(95, "I", "predstojati", "предстоять", "предсто‛јати$C#,#-ји_$MC#предстоя’ть$RV#.#"); list.Add(info);
            info = new ChangeInfo(96, "D", "pretstojnik", "управляющий;руководитель;директор", "пре‛дсто_јни_к$C#м$#.#управля’ющий$RV#;#руководи’тель$RV#;#дире’ктор$RV#.#"); list.Add(info);
            info = new ChangeInfo(97, "I", "predstojnik", "управляющий;руководитель;директор", "пре‛дсто_јни_к$C#м$#.#управля’ющий$RV#;#руководи’тель$RV#;#дире’ктор$RV#.#"); list.Add(info);
            info = new ChangeInfo(98, "D", "pretstraža", "аванпост;сторожевое охранение", "пре“дстра_жа$C#ж$#.#воен$#.#1$D#)#аванпо’ст$RV#;#2$D#)#сторожево’е$RV#охране’ние$RV#.#"); list.Add(info);
            info = new ChangeInfo(99, "I", "predstraža", "аванпост;сторожевое охранение", "пре“дстра_жа$C#ж$#.#воен$#.#1$D#)#аванпо’ст$RV#;#2$D#)#сторожево’е$RV#охране’ние$RV#.#"); list.Add(info);
            info = new ChangeInfo(100, "D", "pretstražni", "сторожевой;относящийся к сторожевому охранению", "пре“дстра_жн||и_$C#,#~а_$C#,#~о_$C#воен$#.#сторожево’й$RV#,#относя’щийся$RV#к$RV#сторожево’му$RV#охране’нию$RV#;#~и$C#распоред$CL#расположе’ние$#сторожево’го$#охране’ния$#.#"); list.Add(info);
            info = new ChangeInfo(101, "I", "predstražni", "сторожевой;относящийся к сторожевому охранению", "пре“дстра_жн||и_$C#,#~а_$C#,#~о_$C#воен$#.#сторожево’й$RV#,#относя’щийся$RV#к$RV#сторожево’му$RV#охране’нию$RV#;#~и$C#распоред$CL#расположе’ние$#сторожево’го$#охране’ния$#.#"); list.Add(info);
            info = new ChangeInfo(102, "D", "pretsmrtni", "предсмертный", ""); list.Add(info);
            */
            ProcessChanges(list, new Repository());
        }

        private static void ProcessChanges(List<ChangeInfo> cList, IRepository repository)
        {
            if (cList.Count == 0) 
                return;

            repository.StartTransaction();

            try
            {
                int lastChangeId = cList[0].id;
                for (int i = 0; i < cList.Count; i++)
                {
                    ChangeInfo c = cList[i];
                    if (lastChangeId < c.id)
                        lastChangeId = c.id;

                    System.Diagnostics.Debug.WriteLine(c.ToString());

                    // Таблица words
                    switch (c.type)
                    {
                        case "I":
                            int cnt = repository.IsExistsWord(Utils.CyrToLat(c.name)); 
                            if (cnt == 0)
                            {
                                repository.InsertWord(c.ToWord2());
                            }
                            // Внести изменения в таблицу letters
                            ChangeLettersAfterInsert(repository, Utils.LatToCyr(c.name));
                            break;

                        case "U":
                            repository.UpdateWord(c.ToWord2());
                            break;

                        case "D":
                            repository.DeleteWord(c.ToWord2());
                            // Внести изменения в таблицу letters
                            ChangeLettersAfterDelete(repository, Utils.LatToCyr(c.name));
                            break;
                    }

                    // Таблица ruswords
                    if (c.type == "I" || c.type == "U")
                    {
                        if (!String.IsNullOrEmpty(c.kw))
                        {
                            string[] keys = c.kw.Split(';');
                            foreach (string key_raw in keys)
                            {
                                if (!String.IsNullOrEmpty(key_raw))
                                {
                                    string key = ClearStresses(key_raw);
                                    int cnt = repository.IsExistsRusWord(key);
                                    if (cnt == 0)
                                    {
                                        // нет записи с таким русским словом
                                        repository.InsertRusWord(new RusWord(key, key_raw, Utils.LatToCyr(c.name)));
                                    }
                                    else
                                    {
                                        // есть запись с таким русским словом
                                        string oldSrbname = "", newSrbname = "";

                                        RusWord rusWord = repository.GetRusWord(key);
                                        if (rusWord != null)
                                        {
                                            oldSrbname = rusWord.srbname;
                                        }

                                        if (String.IsNullOrEmpty(oldSrbname))
                                            newSrbname = Utils.LatToCyr(c.name);
                                        else
                                        {
                                            // Проверить, нет ли уже этого слова (c.name) в строке oldSrbname
                                            bool isExists = false;
                                            string[] tmp = oldSrbname.Split(';');

                                            for (int k = 0; k < tmp.Length; k++)
                                            {
                                                if (tmp[k] == Utils.LatToCyr(c.name))
                                                {
                                                    isExists = true;
                                                    break;
                                                }
                                            }

                                            List<String> listTmp = new List<string>(tmp);

                                            // Если слова нет, то добавить его: добавить в список, отсортировать его и потом - в newSrbname
                                            if (!isExists)
                                            {
                                                listTmp.Add(Utils.LatToCyr(c.name));
                                            }

                                            listTmp.Sort(StringComparer.Create(new CultureInfo("ru-RU"), true));
                                            for (int j = 0; j < listTmp.Count; j++)
                                            {
                                                newSrbname += listTmp[j];
                                                if (j < listTmp.Count - 1)
                                                    newSrbname += ";";
                                            }

                                            // Так было без сортировки
                                            //newSrbname = oldSrbname + ";" + Utils.LatToCyr(c.name);
                                        }

                                        if (!String.IsNullOrEmpty(newSrbname))
                                            repository.UpdateRusWord(new RusWord(key, key_raw, newSrbname));
                                    }
                                }
                            }
                        }
                    }
                    else if (c.type == "D")
                    {
                        // если слово удалили, то нужно вычистить его ключевые слова ruswords
                        string[] keys = c.kw.Split(';');
                        foreach (string key_raw in keys)
                        {
                            if (!String.IsNullOrEmpty(key_raw))
                            {
                                string key = ClearStresses(key_raw);
                                string srbname = "";

                                RusWord rusWord = repository.GetRusWord(key);
                                if (rusWord != null)
                                {
                                    srbname = rusWord.srbname;
                                }

                                if (!String.IsNullOrEmpty(srbname))
                                {
                                    // убрать из srbname c.name
                                    string[] tmp = srbname.Split(';');
                                    string removedKeySrbname = "";
                                    for (int k = 0; k < tmp.Length; k++)
                                    {
                                        if (tmp[k] != Utils.LatToCyr(c.name))
                                        {
                                            removedKeySrbname += tmp[k] + ";";
                                        }
                                    }

                                    // Убрать последний разделитель ;
                                    if (!String.IsNullOrEmpty(removedKeySrbname))
                                    {
                                        if (removedKeySrbname.Substring(removedKeySrbname.Length - 1) == ";")
                                            removedKeySrbname = removedKeySrbname.Substring(0, removedKeySrbname.Length - 1);
                                    }

                                    // Если удаляемое слово было не последним в списке, то обновить запись
                                    if (!String.IsNullOrEmpty(removedKeySrbname))
                                    {
                                        repository.UpdateRusWord(new RusWord(key, key_raw, removedKeySrbname));
                                    }
                                    else
                                    {
                                        // иначе, если поле srbwords.srbname пусто - удалить запись
                                        repository.DeleteRusWord(new RusWord(key, key_raw, ""));
                                    }
                                }
                            }
                        } // for key
                    } // "D" ruswords
                    else if (c.type == "R")
                    {
                        // Update конкретно RusWord
                        repository.UpdateRusWord(new RusWord(c.name, c.stress, c.kw));
                    }
                    else if (c.type == "E")
                    {
                        // Delete конкретно из RusWord (ошибочное ключевое слово, например: 'вместимосгь')
                        repository.DeleteRusWord(new RusWord(c.name, c.stress, c.kw));
                    }
                    else if (c.type == "A")
                    {
                        // Добавление в таблицу RusRef (эта таблица не используется в desktop-версии, но всё равно пусть синхронизируется с мобильной)
                        int cnt = repository.IsExistsRusRef(c.ToRusRef());
                        if (cnt == 0)
                        {
                            repository.InsertRusRef(c.ToRusRef());
                        }
                    }
                    else if (c.type == "X")
                    {
                        // Удаление из таблицы RusRef (эта таблица не используется в desktop-версии, но всё равно пусть синхронизируется с мобильной)
                        repository.DeleteRusRef(c.ToRusRef());
                    }

                } // end for cList

                // Записать ID последнего изменения
                repository.UpdateChanges(lastChangeId);

                repository.CommitTransaction();
            }
            catch
            {
                repository.RollbackTransaction();
            }
        }

        /// <summary>
        /// Исправления в таблице LETTERS после добавления сербского слова.
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="word"></param>
        static void ChangeLettersAfterInsert(IRepository repository, string word)
        {
            SortedDictionary<string, string> dictValue;
            SortedDictionary<string, string> dictOld = new SortedDictionary<string, string>();
            SortedDictionary<string, string> dictNew = new SortedDictionary<string, string>();

            string slovoNext = word;
            string slovo = word;

            while (slovo.Length > 0)
            {
                // Нашли слово
                if (repository.LetterFind(slovo))
                {
                    if (slovo.Equals(word))
                    {
                        return;
                    }

                    // получить значение поля LETTERS.value для слова slovo
                    string letterValue = repository.GetLetterValue(slovo).value;

                    // получить коллекцию морфем
                    dictValue = GetValue4Dict(letterValue);

                    ICollection<string> keys = dictValue.Keys;

                    // проверить, нет ли среди морфем слова word
                    bool isWord = false;
                    bool isValue0 = false;
                    foreach (string key in keys)
                    {
                        if (key.Equals(word))
                        {
                            isWord = true;
                            string v = dictValue[key];
                            if (v == "0")
                            {
                                isValue0 = true;
                            }
                            break;
                        }
                    }

                    if (isWord)
                    {
                        if (isValue0)
                        {
                            foreach (string key in keys)
                            {
                                if (key.Equals(word))
                                {
                                    dictNew.Add(key, "2");
                                }
                                else
                                {
                                    dictNew.Add(key, dictValue[key]);
                                }
                            }

                            // Сделать update найденного слова : записать коллецию в таблицу LETTERS.value
                            repository.UpdateLetters(new Letters(slovo, SetDict2Value(dictNew)));
                        }

                        return;
                    }

                    // пройти по dictValue, проверить все морфемы на полное вхождение в них slovoNext
                    foreach (string key in keys)
                    {
                        // есть полное вхождение
                        if (key.StartsWith(slovoNext))
                        {
                            // перенести эту морфему в новую коллекцию
                            dictNew.Add(key, dictValue[key]);
                        }
                        else
                        {
                            // оставить в старой коллекции
                            dictOld.Add(key, dictValue[key]);
                        }
                    }

                    // Добавить в коллекцию slovoNext
                    dictOld.Add(slovoNext, slovoNext.Equals(word) ? "1" : "0");

                    // Сделать update найденного слова : записать коллецию в таблицу LETTERS.value
                    repository.UpdateLetters(new Letters(slovo, SetDict2Value(dictOld)));

                    // Если slovoNext не равно собственно слову word, то добавить новую запись в таблицу 
                    if (!slovoNext.Equals(word))
                    {
                        // Предварительно добавим собственно слово word
                        dictNew.Add(word, "2");
                    }
                    if (dictNew.Count > 0)
                    {
                        repository.InsertLetters(new Letters(slovoNext, SetDict2Value(dictNew)));
                    }

                    return;
                }
                else // не нашли слово
                {
                    slovoNext = slovo;
                    slovo = slovo.Substring(0, slovo.Length - 1);
                }
            }
        }

        /// <summary>
        /// Исправления в таблице LETTERS после удаления сербского слова.
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="word"></param>
        static void ChangeLettersAfterDelete(IRepository repository, string word)
        {
            SortedDictionary<string, string> dictValue;
            SortedDictionary<string, string> dictNew = new SortedDictionary<string, string>();

            string slovo = word;

            while (slovo.Length > 0)
            {
                // Нашли слово и оно не word (если удаляемое слово содержит морфемы - эту запись нельзя удалять)
                if (repository.LetterFind(slovo) && !slovo.Equals(word))
                {
                    // получить значение поля LETTERS.value для слова slovo
                    string letterValue = repository.GetLetterValue(slovo).value;

                    // получить коллекцию морфем
                    dictValue = GetValue4Dict(letterValue);

                    ICollection<string> keys = dictValue.Keys;

                    // проверить, нет ли среди морфем слова word
                    bool isWord = false;
                    foreach (string key in keys)
                    {
                        if (key.Equals(word))
                        {
                            isWord = true;
                            break;
                        }
                    }

                    if (isWord)
                    {
                        foreach (string key in keys)
                        {
                            if (key.Equals(word))
                            {
                                // если эта морфема есть в таблице в виде ключа
                                if (repository.LetterFind(key))
                                {
                                    if (dictValue[key] == "1")
                                    {
                                        dictNew.Add(key, "0");

                                    }
                                    else if (dictValue[key] == "0")
                                    {
                                        dictNew.Add(key, dictValue[key]);
                                    }
                                    // а если == 2, то убрать из морфем
                                }
                            }
                            else
                            {
                                dictNew.Add(key, dictValue[key]);
                            }
                        }

                        if (dictNew.Count > 0)
                        {
                            // Сделать update найденного слова : записать коллецию в таблицу LETTERS.value
                            repository.UpdateLetters(new Letters(slovo, SetDict2Value(dictNew)));
                        }
                        else
                        {
                            // Если морфем нет, то удалить запись
                            repository.DeleteLetters(new Letters(slovo, ""));

                            // и удалить slovo из морфем в другой записи
                            ChangeLettersAfterDelete(repository, slovo);
                        }
                    }

                    return;
                }
                else // не нашли слово
                {
                    slovo = slovo.Substring(0, slovo.Length - 1);
                }
            }
        }

        static SortedDictionary<string, string> GetValue4Dict(string value)
        {
            SortedDictionary<string, string> dictValue = new SortedDictionary<string, string>();

            string[] morfems = value.Split('|');
            for (int i = 0; i < morfems.Length; i++)
            {
                if (!String.IsNullOrEmpty(morfems[i]))
                {
                    string[] pair = morfems[i].Split(',');
                    dictValue.Add(pair[0], pair[1]);
                }
            }

            return dictValue;
        }

        static string SetDict2Value(SortedDictionary<string, string> dict)
        {
            string result = "";

            ICollection<string> keys = dict.Keys;
            foreach (string key in keys)
            {
                result += key + "," + dict[key] + '|';
            }
         
            return result;
        }

        static string ClearStresses(string text)
        {
            return text
                .Replace('\u201C'.ToString(), "") //[краткое нисходящее]
                .Replace('\u2019'.ToString(), "") //[долгое восходящее]
                .Replace('\u201B'.ToString(), "") //[краткое восходящее]
                .Replace('\u005E'.ToString(), "") //[долгое нисходящее]
                .Replace("_", "")  //заударное
                .Replace("'", ""); //[простое === долгое восходящее]
        }
    }
}
