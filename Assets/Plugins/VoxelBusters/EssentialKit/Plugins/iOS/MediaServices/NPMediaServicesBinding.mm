//
//  NPMediaServicesBinding.mm
//  Native Plugins
//
//  Created by Ashwin kumar on 22/01/19.
//  Copyright (c) 2019 Voxel Busters Interactive LLP. All rights reserved.
//

#import "NPPhotoLibrary.h"
#import "NPMediaServicesDataTypes.h"
#import "NPKit.h"
#import "NPMediaServices.h"
#import "NPMediaContent.h"

#pragma mark - NPMediaServices Binding

NPBINDING DONTSTRIP void NPMediaServicesRegisterCallbacks(RequestPhotoLibraryAccessNativeCallback requestPhotoLibraryAccessCallback,
                                                          RequestCameraAccessNativeCallback requestCameraAccessCallback,
                                                          PickImageNativeCallback pickImageCallback,
                                                          SaveImageToAlbumNativeCallback saveImageToAlbumCallback)
{
    [NPPhotoLibrary setRequestPhotoLibraryAccessCallback:requestPhotoLibraryAccessCallback];
    [NPPhotoLibrary setRequestCameraAccessCallback:requestCameraAccessCallback];
    [NPPhotoLibrary setPickImageCallback:pickImageCallback];
    [NPPhotoLibrary setSaveImageToAlbumCallback:saveImageToAlbumCallback];
}

NPBINDING DONTSTRIP void NPMediaServicesRequestPhotoLibraryAccess(void* tagPtr)
{
    [[NPPhotoLibrary sharedInstance] requestPhotoLibraryAccess:tagPtr];
}

NPBINDING DONTSTRIP PHAuthorizationStatus NPMediaServicesGetPhotoLibraryAccessStatus()
{
    return [[NPPhotoLibrary sharedInstance] getPhotoLibraryAccessStatus];
}

NPBINDING DONTSTRIP void NPMediaServicesRequestCameraAccess(void* tagPtr)
{
    [[NPPhotoLibrary sharedInstance] requestCameraAccess:tagPtr];
}

NPBINDING DONTSTRIP AVAuthorizationStatus NPMediaServicesGetCameraAccessStatus()
{
    return [[NPPhotoLibrary sharedInstance] getCameraAccessStatus];
}

NPBINDING DONTSTRIP bool NPMediaServicesCanPickImageFromGallery()
{
    return [[NPPhotoLibrary sharedInstance] canPickImageFromGallery];
}

NPBINDING DONTSTRIP void NPMediaServicesPickImageFromGallery(bool canEdit, void* tagPtr)
{
    [[NPPhotoLibrary sharedInstance] pickImageFromGallery:canEdit withTag:tagPtr];
}

NPBINDING DONTSTRIP bool NPMediaServicesCanPickImageFromCamera()
{
    return [[NPPhotoLibrary sharedInstance] canPickImageFromCamera];
}

NPBINDING DONTSTRIP void NPMediaServicesPickImageFromCamera(bool canEdit, void* tagPtr)
{
    [[NPPhotoLibrary sharedInstance] pickImageFromCamera:canEdit withTag:tagPtr];
}

NPBINDING DONTSTRIP bool NPMediaServicesCanSaveImageToAlbum()
{
    return [[NPPhotoLibrary sharedInstance] canSaveImageToAlbum];
}

NPBINDING DONTSTRIP void NPMediaServicesSaveImageToAlbum(const char* albumName, void* imageDataPtr, int imageDataLength, void* tagPtr)
{
    UIImage*    image   = NPCreateImage(imageDataPtr, imageDataLength);
    [[NPPhotoLibrary sharedInstance] saveImageToAlbum:image withTag:tagPtr];
}


NPBINDING DONTSTRIP void NPMediaServicesSelectMediaContent(MediaContentSelectionOptionsData optionsData, void* tagPtr, SelectMediaContentNativeCallback callback)
{
    MediaContentSelectionOptions *options = [[MediaContentSelectionOptions alloc] init];
    options.title = NPCreateNSStringFromCString(optionsData.title);
    options.allowedMimeType = NPCreateNSStringFromCString(optionsData.allowedMimeType);
    options.maxAllowed = optionsData.maxAllowed;
    options.showPermissionDialog = optionsData.showPermissionDialog;
    
    [[NPMediaServices sharedInstance] selectMediaContent:options withCallback:^(NSArray<NPMediaContent *> *contents, NSError *error) {
            
            NPArray *mediaContents = NPCreateNativeArrayFromNSArray(contents);
            callback(mediaContents, NPCreateError(error), tagPtr);
        }];
}

NPBINDING DONTSTRIP void NPMediaServicesCaptureMediaContent(MediaContentCaptureOptionsData optionsData, void* tagPtr, CaptureMediaContentNativeCallback callback)
{
    MediaContentCaptureOptions *options = [[MediaContentCaptureOptions alloc] init];
    options.captureType = (MediaContentCaptureType)optionsData.captureType;
    options.title = NPCreateNSStringFromCString(optionsData.title);
    options.fileName = NPCreateNSStringFromCString(optionsData.fileName);
    
    [[NPMediaServices sharedInstance] captureMediaContent:options withCallback:^(NPMediaContent *content, NSError *error) {
            callback(content ? NPRetainWithOwnershipTransfer(content) : nil, NPCreateError(error), tagPtr);
        }];
}

NPBINDING DONTSTRIP void NPMediaServicesSaveMediaContent(void* bytes, long length, const char* mime, MediaContentSaveOptionsData optionsData, void* tagPtr, SaveMediaContentNativeCallback callback)
{
    MediaContentSaveOptions *options = [[MediaContentSaveOptions alloc] init];
    options.directoryName = NPCreateNSStringFromCString(optionsData.directoryName);
    options.fileName = NPCreateNSStringFromCString(optionsData.fileName);
    
    [[NPMediaServices sharedInstance] saveMediaContent:[NSData dataWithBytes:bytes  length:length]
                                          withMimeType: NPCreateNSStringFromCString(mime)
                                            withOptions:options
                                          withCallback:^(BOOL success, NSError *error) {
                                            callback(success, NPCreateError(error), tagPtr);
    }];
}

#pragma mark - NPMediaContent Binding

NPBINDING DONTSTRIP void NPMediaContentAsRawData(void* mediaContentPtr, void* tagPtr, MediaContentAsRawDataNativeCallback callback)
{
    NPMediaContent*  mediaContent     = (__bridge NPMediaContent*)mediaContentPtr;
    [mediaContent asRawData:^(NSData *data, NSString* mime, NSError *error) {
        callback((void*)[data bytes], (int)[data length], NPCreateCStringFromNSString(mime) ,NPCreateError(error),tagPtr);
    }];
}


NPBINDING DONTSTRIP void NPMediaContentAsFilePath(void* mediaContentPtr, const char* directoryName, const char* fileName, void* tagPtr, MediaContentAsFilePathNativeCallback callback)
{
    NPMediaContent*  mediaContent     = (__bridge NPMediaContent*)mediaContentPtr;
    [mediaContent asFilePath:NPCreateNSStringFromCString(directoryName) withFileName:NPCreateNSStringFromCString(fileName) withCallback:^(NSString *path, NSError *error) {
        callback(NPCreateCStringFromNSString(path), NPCreateError(error), tagPtr);
    }];
}

