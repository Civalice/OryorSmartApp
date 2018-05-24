//
//  LBSHelper.m
//  LBS
//
//  Created by Civalice on 10/20/14.
//  Copyright (c) 2014 Civalice. All rights reserved.
//

#import <Foundation/Foundation.h>
#import "LBSHelper.h"
#import "LBSMapView.h"
//#import "LBSMapViewController.h"
#import <GoogleMaps/GoogleMaps.h>
#import "UnityAppController.h"

extern void UnitySendMessage(const char *, const char *, const char *);

extern "C" {
    void _OpenLBS();
    void _CloseLBS();
}

void _OpenLBS() {
    [LBSHelper OpenLBS];
}

void _CloseLBS() {
    
}

@implementation LBSHelper
static LBSMapView* myView;
UIView *tempView;
+(void) RegisterGMSService
{
    [GMSServices provideAPIKey:@"AIzaSyDGfF9wAFsCI9Cs0mbUiHsSwCJoXLzF-2o"];
}

+(void) OpenLBS
{
    [LBSHelper RegisterGMSService];
    //    myView=[[LBSMapViewController alloc] init];
    UIViewController *sGLView =  [GetAppController() rootViewController];
    //    [sGLView.view addSubview:myView.view];
    tempView = sGLView.view;
    myView=[[LBSMapView alloc] initWithFrame:sGLView.view.bounds];
    sGLView.view = myView;
    //    [sGLView.view addSubview:myView];
}
+(void) CloseLBS
{
    //    [myView removeFromSuperview];
    UIViewController *sGLView =  [GetAppController() rootViewController];
    sGLView.view = tempView;
    UnitySendMessage("LBSLoader", "CallClose", "");
}
@end