//
//  MessageDeliverService.h
//  Text2IM
//
//  Created by H2I on 2018/6/20.
//  Copyright © 2018 sliang. All rights reserved.
//

#import <Foundation/Foundation.h>

@class MessageData;

@protocol MessageReceiveDelegate
-(void) onReceivedOne:(MessageData*)Data;
-(void) onReceivedAll:(NSArray<MessageData*>*)Data;
@end

typedef void(^DataArrivalHandler)(MessageData*);
typedef void(^DataCompeletionHandler)(NSArray<MessageData*>* );

@interface MessageReceiveService : NSObject
-(void) receiveAllMessageData:(id<MessageReceiveDelegate>)WithDelegate;
-(void) receiveNewestMessageData:(id<MessageReceiveDelegate>)WithDelegate;
-(void) markAsDelivered: (MessageData*) messageData;
@end
