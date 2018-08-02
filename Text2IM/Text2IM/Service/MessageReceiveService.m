//
//  MessageDeliverService.m
//  Text2IM
//
//  Created by H2I on 2018/6/20.
//  Copyright © 2018 sliang. All rights reserved.
//

#import "MessageReceiveService.h"
#import "../Model/MessageData.h"

#define RESTful_SERVER_URL @"http://messagedeliver.azurewebsites.net/"

typedef void (^DataHandler)(MessageData*);

@interface MessageReceiveService()
-(NSArray<MessageData*>*) parse:(NSArray*)Data WithResultHandler:(DataHandler)handler;
@end

@implementation MessageReceiveService

-(id) init{
    self = [super init];
    
    return self;
}

- (void)receiveAllMessageData:(id<MessageReceiveDelegate>)WithDelegate {
    NSString* urlAsString = [NSString stringWithFormat:@"%@api/messagedatas", RESTful_SERVER_URL];
    NSCharacterSet* charSet = [NSCharacterSet URLQueryAllowedCharacterSet];
    NSString* encodedUrlAsString = [urlAsString stringByAddingPercentEncodingWithAllowedCharacters:charSet];
    NSURL* dataUrl = [NSURL URLWithString:encodedUrlAsString];
    
    NSURLSession* session = [NSURLSession sharedSession];
    NSMutableURLRequest* request = [NSMutableURLRequest requestWithURL:dataUrl];
    request.HTTPMethod = @"GET";
    
    NSURLSessionDataTask* downloadTask = [session dataTaskWithRequest:request completionHandler:^(NSData* data, NSURLResponse* response, NSError* error){
        if(!error){
            if([response isKindOfClass:[NSHTTPURLResponse class]]){
                NSError* jsonError;
                NSArray* jsonResponse = [NSJSONSerialization JSONObjectWithData:data options:0 error:&jsonError];
                NSArray<MessageData*>* messageDatas = [self parse:jsonResponse WithResultHandler:^(MessageData* messageData){
                    [WithDelegate onReceivedOne:messageData];
                }];
                
                [WithDelegate onReceivedAll:messageDatas];
                NSLog(@"%@", jsonResponse);
            }else{
                NSLog(@"%@",response);
            }
        }else{
            NSLog(@"%@", error.description);
        }
    }];
    
    [downloadTask resume];
}

- (void)receiveNewestMessageData:(id<MessageReceiveDelegate>)WithDelegate {
}

- (void)markAsDelivered:(MessageData *)messageData {

}

- (NSArray<MessageData *> *)parse:(NSArray *)Data WithResultHandler:(DataHandler)handler{
    NSMutableArray<MessageData*> * array = [[NSMutableArray<MessageData*> alloc] init];

    for (int i = 0; i < Data.count; i++) {
        NSDictionary* object = [Data objectAtIndex:i];
        MessageData* messageData = [MessageData make:object];
        handler(messageData);
        [array addObject:messageData];
    }

    return [[NSArray<MessageData*> alloc] initWithArray:array];
}

@end
