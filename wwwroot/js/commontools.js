//
//
//  anaylsis  algorithm   datatime open close low high      updatatime:20201106
//
//
function mytone_max_min_close(kxdata) {

    var maxclose     = 0;
    var minclose     = 999999;
    var highclose    = 0;
    var lowclose     = 999999;
    var kxp          = 0;    
    var hcp          = 0;
    var lcp          = 0;
    var maxclosedate = "";
    var minclosedate = "";
    var highclosedate= "";
    var lowclosedate = "";

    var flu = 0;
    var avgflu = 0;
    var totalflu = 0;
    var fludate;
    var fxp = 0;

    var upc = 0;
    var upcc = 0;
    var dwc = 0;
    var dwcc = 0;
    
    var res = new Array();
    var j = kxdata.length;    
    for (var i = 0; i < j; i++) {

        //maxclose minclose
        kxp = parseFloat(kxdata[i][2]);      
        if (kxp > maxclose) {
            maxclose = kxp;
            maxclosedate = kxdata[i][0];
        }
        if (kxp < minclose) {
            minclose = kxp;
            minclosedate = kxdata[i][0];
        }

        //highest lowest
        hcp = parseFloat(kxdata[i][4]);
        if (hcp > highclose) {
            highclose = hcp;
            highclosedate = kxdata[i][0];
        }
        lcp = parseFloat(kxdata[i][3]);
        if (lcp < lowclose) {
            lowclose = lcp;
            lowclosedate = kxdata[i][0];
        }

        //flcutuation
        fxp = parseFloat(kxdata[i][4]) - parseFloat(kxdata[i][3]);
        if (fxp > flu) {
            flu = fxp;
            fludate = kxdata[i][0];
        }
        totalflu = totalflu + fxp;

        //point up dwon
        if (parseFloat(kxdata[i][2]) >= parseFloat(kxdata[i][1])) {
            upc++;
            upcc = upcc + parseFloat(kxdata[i][2]) - parseFloat(kxdata[i][1]);
        } else {
            dwc++;
            dwcc = dwcc + parseFloat(kxdata[i][1]) - parseFloat(kxdata[i][2]);
        }

    }
    //fluctuation
    avgflu = totalflu / j;

    res.push(parseFloat(maxclose.toFixed(5))); //0
    res.push(maxclosedate);                    //1
    res.push(parseFloat(minclose.toFixed(5))); //2
    res.push(minclosedate);                    //3
    res.push(parseFloat(highclose.toFixed(5)));//4
    res.push(highclosedate);                   //5
    res.push(parseFloat(lowclose.toFixed(5))); //6
    res.push(lowclosedate);                    //7

    res.push(parseFloat(flu.toFixed(5)));      //8
    res.push(fludate);                         //9
    res.push(parseFloat(avgflu.toFixed(5)));   //10

    res.push(upc);                             //11
    res.push(dwc);                             //12
    res.push(parseFloat(upcc.toFixed(5)));     //13
    res.push(parseFloat(dwcc.toFixed(5)));     //14

    return res;
}




function mytone_varfin(kxdata) {

    var beginclose=-999999;
    var endclose=-999999;
    var d=0;    
    var x = 0;  
    var dz = 0;
    var xz = 0;
    var g = 0;
    var f = 0;
    var gz = 0;
    var gzz = 0;
    var gd = 0;
    var gdz = 0;
    var fz = 0;
    var fzz = 0;
    var fd = 0;
    var fdz = 0;
    var flg = 0;
    var flp = 0;

    var gzt  = 0;   //actual>forecast and actual>0 的跌次数
    var gdt  = 0;   //actual<forecast and actual<0 的涨次数
    var gztz = 0;   //actual>forecast and actual>0 的跌的总点数
    var gdtz = 0;   //actual<forecast and actual<0 的涨的总点数

    var gztf  = 0;   //actual>forecast and actual>0 的涨次数
    var gdtf  = 0;   //actual<forecast and actual<0 的跌次数
    var gztzf = 0;   //actual>forecast and actual>0 的涨的总点数
    var gdtzf = 0;   //actual<forecast and actual<0 的跌的总点数

    var bz = 0;      //actual>0 涨的次数
    var bd = 0;      //actual>0 跌的次数
    var cz = 0;      //actual<0 涨的次数
    var cd = 0;      //actual<0 跌的次数
    var flx = 0;

    var res = new Array();
    var j = kxdata.length;
    var p = 0;
    var i = 0;
    for (i = 1; i < j; i++) { 

        if ((kxdata[i][8] != "" && kxdata[i][8] != null) || (i == j - 1)) {
            if (i == j - 1) {
                endclose = parseFloat(kxdata[i][2]);
            } else {
                endclose = parseFloat(kxdata[i - 1][2]);
            }
            if (beginclose != -999999 ) {
                if (endclose > beginclose) {  //up///////////////////////////////////
                    d++;
                    dz = dz + endclose - beginclose;
                    if (flg > 0) {   // actual>forecast up
                        gz++;
                        gzz = gzz + endclose - beginclose;     
                        if (flp > 0) {  //actual>0 actual>forecast up
                            gztf++;
                            gztzf = gztzf + endclose - beginclose;
                        }
                    }
                    if (flg < 0) { //actual<forecast 
                        fz++;
                        fzz = fzz + endclose - beginclose;
                        if (flp < 0) {  //actual<0  actual<forecast  up
                            gdt++;
                            gdtz = gdtz + endclose-beginclose;
                        }
                    }
                    if (flx > 0) { //actual>0 up
                        bz++;
                    } else {  //actual<0 up
                        cz++;
                    }

                } else {                      //down////////////////////////////////////
                    x++;
                    xz = xz + beginclose - endclose;
                    if (flg > 0) {  //actual>forecast   
                        gd++;
                        gdz = gdz + beginclose - endclose;
                        if (flp > 0) { //actual>0 actual>forecast down
                            gzt++;
                            gztz = gztz + beginclose - endclose;
                        }
                    }
                    if (flg < 0) {  //actual<forecast down
                        fd++;
                        fdz = fdz + beginclose - endclose;
                        if (flp < 0) {//actual<0  actual<forecast down
                            gdtf++;
                            gdtzf = gdtzf + beginclose - endclose;
                        }
                    }
                    if (flx < 0) { //actual<0 down
                        cd++;
                    } else {  //actual>0 down
                        bd++;
                    }
                }             
            }

            beginclose = parseFloat(kxdata[i][2]);
            flg = parseFloat(stringlx(kxdata[i][8])) - parseFloat(stringlx(kxdata[i][7]));
            if (  flg>0 ) {     // actual>forecast
                g++;
            } else if (flg<0){  // actual<forecast
                f++;
            }
            flp = parseFloat(stringlx(kxdata[i][8]));  
            flx = parseFloat(stringlx(kxdata[i][8]));
        }          
    }
    res.push(d);                          //所有到下个公布日的close 为正的数量    0
    res.push(x);                          //所有到下个公布日的close 为负的数量    1 
    res.push(parseFloat(dz.toFixed(5)));  //公布后 到下个公布日之间的总涨点2
    res.push(parseFloat(xz.toFixed(5)));  //公布后 到下个公布日之间的总跌点3
    res.push(g);                          //4
    res.push(f);//5
    res.push(gz);                         //公布后涨的次数6
    res.push(gd); //7
    res.push(fz);//8
    res.push(fd);//9
    res.push(parseFloat(gzz.toFixed(5)));//10
    res.push(parseFloat(gdz.toFixed(5)));//11
    res.push(parseFloat(fzz.toFixed(5)));//12
    res.push(parseFloat(fdz.toFixed(5)));//13

    res.push(gzt);//14
    res.push(gdt);//15
    res.push(parseFloat(gztz.toFixed(5)));//16
    res.push(parseFloat(gdtz.toFixed(5)));//17

    res.push(gztf);//18
    res.push(gdtf);//19
    res.push(parseFloat(gztzf.toFixed(5)));//20
    res.push(parseFloat(gdtzf.toFixed(5)));//21

    res.push(bz);//22
    res.push(bd);//23
    res.push(cz);//24
    res.push(cd);//25

    return res;
}

function stringlx(tt) {
    if (tt != null || tt != "") {
        tt = tt.replace("M", "");
        tt = tt.replace("%", "");
        tt = tt.replace("HK", "");
        tt = tt.replace("Kr", "");
        tt = tt.replace("B", "");
        tt = tt.replace("$", "");
        tt = tt.replace("K", "");
        tt = tt.replace("£", "");
        tt = tt.replace("€", "");
        tt = tt.replace("R", "");
    }
    return tt;
}



