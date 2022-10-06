using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace CitrusDammakuCount.BilibiliAPI.Season
{
    [Serializable]
    public class Rootobject
    {
        public int code;
        public string message;
        public Result result;
    }

    [Serializable]
    public class Result
    {
        public Activity activity;
        public string alias;
        public Area[] areas;
        public string bkg_cover;
        public string cover;
        public Episode[] episodes;
        public string evaluate;
        public Freya freya;
        public string jp_title;
        public string link;
        public int media_id;
        public int mode;
        public New_Ep new_ep;
        public Payment payment;
        public Positive positive;
        public Publish publish;
        public Rating rating;
        public string record;
        public Rights rights;
        public int season_id;
        public string season_title;
        public Season[] seasons;
        public Section[] section;
        public Series series;
        public string share_copy;
        public string share_sub_title;
        public string share_url;
        public Show show;
        public int show_season_type;
        public string square_cover;
        public Stat stat;
        public int status;
        public string subtitle;
        public string title;
        public int total;
        public int type;
        public User_Status user_status;
    }

    [Serializable]
    public class Activity
    {
        public string head_bg_url;
        public int id;
        public string title;
    }

    [Serializable]
    public class Freya
    {
        public string bubble_desc;
        public int bubble_show_cnt;
        public int icon_show;
    }

    [Serializable]
    public class New_Ep
    {
        public string desc;
        public int id;
        public int is_new;
        public string title;
    }

    [Serializable]
    public class Payment
    {
        public int discount;
        public Pay_Type pay_type;
        public string price;
        public string promotion;
        public string tip;
        public int view_start_time;
        public int vip_discount;
        public string vip_first_promotion;
        public string vip_promotion;
    }

    [Serializable]
    public class Pay_Type
    {
        public int allow_discount;
        public int allow_pack;
        public int allow_ticket;
        public int allow_time_limit;
        public int allow_vip_discount;
        public int forbid_bb;
    }

    [Serializable]
    public class Positive
    {
        public int id;
        public string title;
    }

    [Serializable]
    public class Publish
    {
        public int is_finish;
        public int is_started;
        public string pub_time;
        public string pub_time_show;
        public int unknow_pub_date;
        public int weekday;
    }

    [Serializable]
    public class Rating
    {
        public int count;
        public float score;
    }

    [Serializable]
    public class Rights
    {
        public int allow_bp;
        public int allow_bp_rank;
        public int allow_download;
        public int allow_review;
        public int area_limit;
        public int ban_area_show;
        public int can_watch;
        public string copyright;
        public int forbid_pre;
        public int freya_white;
        public int is_cover_show;
        public int is_preview;
        public int only_vip_download;
        public string resource;
        public int watch_platform;
    }

    [Serializable]
    public class Series
    {
        public int series_id;
        public string series_title;
    }

    [Serializable]
    public class Show
    {
        public int wide_screen;
    }

    [Serializable]
    public class Stat
    {
        public int coins;
        public int danmakus;
        public int favorite;
        public int favorites;
        public int likes;
        public int reply;
        public int share;
        public int views;
    }

    [Serializable]
    public class User_Status
    {
        public int area_limit;
        public int ban_area_show;
        public int follow;
        public int follow_status;
        public int login;
        public int pay;
        public int pay_pack_paid;
        public int sponsor;
    }

    [Serializable]
    public class Area
    {
        public int id;
        public string name;
    }

    [Serializable]
    public class Episode
    {
        public int aid;
        public string badge;
        public Badge_Info badge_info;
        public int badge_type;
        public string bvid;
        public int cid;
        public string cover;
        public Dimension dimension;
        public int duration;
        public string from;
        public int id;
        public bool is_view_hide;
        public string link;
        public string long_title;
        public int pub_time;
        public int pv;
        public string release_date;
        public Rights1 rights;
        public string share_copy;
        public string share_url;
        public string short_link;
        public int status;
        public string subtitle;
        public string title;
        public string vid;
    }

    [Serializable]
    public class Badge_Info
    {
        public string bg_color;
        public string bg_color_night;
        public string text;
    }

    [Serializable]
    public class Dimension
    {
        public int height;
        public int rotate;
        public int width;
    }

    [Serializable]
    public class Rights1
    {
        public int allow_demand;
        public int allow_dm;
        public int allow_download;
        public int area_limit;
    }

    [Serializable]
    public class Season
    {
        public string badge;
        public Badge_Info1 badge_info;
        public int badge_type;
        public string cover;
        public string horizontal_cover_1610;
        public string horizontal_cover_169;
        public int media_id;
        public New_Ep1 new_ep;
        public int season_id;
        public string season_title;
        public int season_type;
        public Stat1 stat;
    }

    [Serializable]
    public class Badge_Info1
    {
        public string bg_color;
        public string bg_color_night;
        public string text;
    }

    [Serializable]
    public class New_Ep1
    {
        public string cover;
        public int id;
        public string index_show;
    }

    [Serializable]
    public class Stat1
    {
        public int favorites;
        public int series_follow;
        public int views;
    }

    [Serializable]
    public class Section
    {
        public int attr;
        public int episode_id;
        public Episode1[] episodes;
        public int id;
        public Report report;
        public string title;
        public int type;
    }

    [Serializable]
    public class Report
    {
        public string season_id;
        public string season_type;
        public string sec_title;
        public string section_id;
        public string section_type;
    }

    [Serializable]
    public class Episode1
    {
        public int aid;
        public string badge;
        public Badge_Info2 badge_info;
        public int cid;
        public string cover;
        public int id;
        public bool is_view_hide;
        public string link;
        public int pub_time;
        public int pv;
        public Report1 report;
        public int status;
        public string title;
    }

    [Serializable]
    public class Badge_Info2
    {
        public string bg_color;
        public string bg_color_night;
        public string text;
    }

    [Serializable]
    public class Report1
    {
        public string aid;
        public string ep_title;
        public string position;
        public string season_id;
        public string season_type;
        public string section_id;
        public string section_type;
    }
}
