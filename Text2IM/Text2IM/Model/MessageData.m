//
//  MessageData.m
//  Text2IM
//
//  Created by H2I on 2018/6/20.
//  Copyright © 2018 sliang. All rights reserved.
//

#import "MessageData.h"

#define SENDER_PROPERTY_KEY @"sender"
#define TIMESTAMP_PROPERTY_KEY @"timestamp"
#define PUBLISHER_DISPLAY_NAME_PROPERTY_KEY @"publisherDisplayName"
#define MESSAGE_ID_PROPERTY_KEY @"messageId"
#define BODY_PROPERTY_KEY @"body"
#define PUBLISHER_ID_PROPERTY_KEY @"publisherId"

@implementation MessageData

+ (MessageData *)make:(NSDictionary *)fromJson { 
    MessageData* messageData = [[MessageData alloc] init];
    messageData.sender = [fromJson valueForKey:SENDER_PROPERTY_KEY];
    messageData.body = [fromJson valueForKey:BODY_PROPERTY_KEY];
    messageData.messageGuid = [fromJson valueForKey:MESSAGE_ID_PROPERTY_KEY];
    messageData.publisherName = [fromJson valueForKey:PUBLISHER_ID_PROPERTY_KEY];
    
    NSDateFormatter* dateFormatter = [[NSDateFormatter alloc] init];
    dateFormatter.timeZone = [NSTimeZone timeZoneWithName:@"UTC"]; /*The DateFormatter always assume the timezone is not UTC. But we actually pass in the UTC time*/
    dateFormatter.dateFormat = @"yyyy-MM-dd'T'HH:mm:ss";
    NSString* dateString = [fromJson valueForKey:TIMESTAMP_PROPERTY_KEY];
    messageData.timestamp = [dateFormatter dateFromString:dateString];

    return messageData;
}

-(bool) equals:(MessageData*)otherObject{
    return [self.messageGuid compare:otherObject.messageGuid] == NSOrderedSame;
}

@end
