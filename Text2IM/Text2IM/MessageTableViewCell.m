//
//  MessageTableViewCell.m
//  Text2IM
//
//  Created by H2I on 2018/6/20.
//  Copyright © 2018 sliang. All rights reserved.
//

#import "MessageTableViewCell.h"
#import "Model/MessageData.h"

@implementation MessageTableViewCell

- (void)awakeFromNib {
    [super awakeFromNib];
    // Initialization code
}

- (void)setSelected:(BOOL)selected animated:(BOOL)animated {
    [super setSelected:selected animated:animated];

    // Configure the view for the selected state
}

-(id) init{
    self = [super init];
    
    return self;
}

- (void)updateView:(MessageData *)WithData {
    self.senderLable.text = WithData.sender;
    self.messagePreviewLabel.text = WithData.body;
    
    NSDateFormatter* dateFormatter = [[NSDateFormatter alloc] init];
    [dateFormatter setFormatterBehavior:NSDateFormatterBehavior10_4];
    [dateFormatter setDateStyle:NSDateFormatterShortStyle];
    [dateFormatter setTimeStyle:NSDateFormatterShortStyle];
    [dateFormatter setTimeZone:[NSTimeZone localTimeZone]];
    
    self.tiemstampLabel.text = [dateFormatter stringFromDate:WithData.timestamp];
    
}

@end
