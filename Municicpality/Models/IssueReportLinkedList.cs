namespace Municicpality.Models
{
    // Node representing a single issue report in the linked list
    //Microsoft Learn. (n.d.). LinkedList<T> Class (System.Collections.Generic). Retrieved 10 September 2025, from Microsoft Learn website: https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.linkedlist-1?view=net-9.0
    public class IssueReportNode
    {
        public int Id { get; set; } // Unique ID for the report
        public string Location { get; set; } = string.Empty; // Reported issue location
        public string Category { get; set; } = string.Empty; // Issue category
        public string Description { get; set; } = string.Empty; // Issue description
        public string AttachmentPath { get; set; } = string.Empty; // Optional file attachment path
        public DateTime SubmittedAt { get; set; } // Timestamp of submission
        public IssueReportNode? Next { get; set; } // Pointer to next node
    }

    // In-memory linked list for storing issue reports
    public class IssueReportLinkedList
    {
        private IssueReportNode? head; // Head node of the list
        private int nextId = 1; // Auto-incrementing ID

        // Public getter for the head node
        public IssueReportNode? Head => head; // Allows read-only access to the list head

        // Add a new report to the end of the list
        public IssueReportNode AddReport(string location, string category, string description, string attachmentPath)
        {
            var newNode = new IssueReportNode
            {
                Id = nextId++, // Assign incremental ID
                Location = location,
                Category = category,
                Description = description,
                AttachmentPath = attachmentPath,
                SubmittedAt = DateTime.Now // Capture submission time
            };

            // Insert node at head if list is empty, otherwise traverse to end
            if (head == null)
            {
                head = newNode;
            }
            else
            {
                var current = head;
                while (current.Next != null) current = current.Next;
                current.Next = newNode;
            }

            return newNode; // Return the newly added node
        }

        // Enumerate all nodes in the list
        public IEnumerable<IssueReportNode> GetAll()
        {
            var current = head;
            while (current != null)
            {
                yield return current;
                current = current.Next;
            }
        }

        // Clear the linked list
        public void Clear()
        {
            head = null; // Remove all nodes
            nextId = 1; // Reset ID counter
        }
    }
}
