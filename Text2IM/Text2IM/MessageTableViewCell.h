//
//  MessageTableViewCell.h
//  Text2IM
//
//  Created by H2I on 2018/6/20.
//  Copyright © 2018 sliang. All rights reserved.
//

#import <UIKit/UIKit.h>

@class MessageData;

@interface MessageTableViewCell : UITableViewCell
@property (weak, nonatomic) IBOutlet UILabel *tiemstampLabel;
@property (weak, nonatomic) IBOutlet UILabel *senderLable;
@property (weak, nonatomic) IBOutlet UILabel *messagePreviewLabel;

-(void) updateView:(MessageData*)WithData;
@end
