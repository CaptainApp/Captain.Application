using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Captain.Application {
  /// <inheritdoc cref="Stream" />
  /// <summary>
  ///   Provides a container for multiple underlying streams
  /// </summary>
  internal sealed class MultiStream : Stream, IList<Stream> {
    /// <summary>
    ///   Stream enumeration
    /// </summary>
    private readonly List<Stream> streams = new List<Stream>();

    #region IList<T> properties

    /// <inheritdoc />
    /// <summary>Gets or sets the element at the specified index</summary>
    /// <param name="index">The zero-based index of the element to get or set</param>
    /// <returns>The element at the specified index</returns>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    ///   <paramref name="index" /> is not a valid index in the <see cref="T:System.Collections.Generic.IList`1" />
    /// </exception>
    /// <exception cref="T:System.NotSupportedException">
    ///   The property is set and the
    ///   <see cref="T:System.Collections.Generic.IList`1" /> is read-only
    /// </exception>
    public Stream this[int index] {
      get => this.streams[index];
      set => this.streams[index] = value;
    }

    #endregion

    #region IDisposable methods

    /// <inheritdoc />
    /// <summary>
    ///   Releases the unmanaged resources used by the <see cref="T:System.IO.Stream" /> and optionally releases the
    ///   managed resources
    /// </summary>
    /// <param name="disposing">
    ///   true to release both managed and unmanaged resources; false to release only unmanaged
    ///   resources
    /// </param>
    protected override void Dispose(bool disposing) {
      this.streams.ForEach(s => s.Dispose());
      base.Dispose(disposing);
    }

    #endregion

    #region Stream properties

    /// <inheritdoc />
    /// <summary>
    ///   When overridden in a derived class, gets a value indicating whether the current stream supports reading
    /// </summary>
    /// <returns>true if the stream supports reading; otherwise, false</returns>
    public override bool CanRead => this.streams.Any(s => s.CanRead);

    /// <inheritdoc />
    /// <summary>
    ///   When overridden in a derived class, gets a value indicating whether the current stream supports seeking
    /// </summary>
    /// <returns>true if the stream supports seeking; otherwise, false</returns>
    public override bool CanSeek => this.streams.All(s => s.CanSeek);

    /// <inheritdoc />
    /// <summary>
    ///   When overridden in a derived class, gets a value indicating whether the current stream supports writing
    /// </summary>
    /// <returns>true if the stream supports writing; otherwise, false</returns>
    public override bool CanWrite => this.streams.All(s => s.CanWrite);

    /// <inheritdoc />
    /// <summary>When overridden in a derived class, gets the length in bytes of the stream</summary>
    /// <returns>A long value representing the length of the stream in bytes</returns>
    /// <exception cref="T:System.NotSupportedException">A class derived from Stream does not support seeking</exception>
    /// <exception cref="T:System.ObjectDisposedException">Methods were called after the stream was closed</exception>
    public override long Length => this.streams.First().Length;

    /// <inheritdoc />
    /// <summary>When overridden in a derived class, gets or sets the position within the current stream</summary>
    /// <returns>The current position within the stream</returns>
    /// <exception cref="T:System.IO.IOException">An I/O error occurs</exception>
    /// <exception cref="T:System.NotSupportedException">The stream does not support seeking</exception>
    /// <exception cref="T:System.ObjectDisposedException">Methods were called after the stream was closed</exception>
    public override long Position {
      get => this.streams.First().Position;
      set => this.streams.ForEach(s => s.Position = value);
    }

    #endregion

    #region Stream methods

    /// <inheritdoc />
    /// <summary>
    ///   When overridden in a derived class, clears all buffers for this stream and causes any buffered data to be
    ///   written to the underlying device
    /// </summary>
    /// <exception cref="T:System.IO.IOException">An I/O error occurs</exception>
    public override void Flush() => this.streams.ForEach(s => s.Flush());

    /// <inheritdoc />
    /// <summary>When overridden in a derived class, sets the position within the current stream</summary>
    /// <param name="offset">A byte offset relative to the <paramref name="origin" /> parameter</param>
    /// <param name="origin">
    ///   A value of type <see cref="T:System.IO.SeekOrigin" /> indicating the reference point used to
    ///   obtain the new position
    /// </param>
    /// <returns>The new position within the current stream</returns>
    /// <exception cref="T:System.IO.IOException">An I/O error occurs</exception>
    /// <exception cref="T:System.NotSupportedException">
    ///   The stream does not support seeking, such as if the stream is
    ///   constructed from a pipe or console output
    /// </exception>
    /// <exception cref="T:System.ObjectDisposedException">Methods were called after the stream was closed</exception>
    public override long Seek(long offset, SeekOrigin origin) {
      this.streams.ForEach(s => { s.Seek(offset, origin); });
      return this.streams.First().Position;
    }

    /// <inheritdoc />
    /// <summary>When overridden in a derived class, sets the length of the current stream</summary>
    /// <param name="value">The desired length of the current stream in bytes</param>
    /// <exception cref="T:System.IO.IOException">An I/O error occurs</exception>
    /// <exception cref="T:System.NotSupportedException">
    ///   The stream does not support both writing and seeking, such as if the
    ///   stream is constructed from a pipe or console output
    /// </exception>
    /// <exception cref="T:System.ObjectDisposedException">Methods were called after the stream was closed</exception>
    public override void SetLength(long value) => this.streams.ForEach(s => s.SetLength(value));

    /// <inheritdoc />
    /// <summary>
    ///   When overridden in a derived class, reads a sequence of bytes from the current stream and advances the
    ///   position within the stream by the number of bytes read
    /// </summary>
    /// <param name="buffer">
    ///   An array of bytes. When this method returns, the buffer contains the specified byte array with the
    ///   values between <paramref name="offset" /> and (<paramref name="offset" /> + <paramref name="count" /> - 1) replaced
    ///   by the bytes read from the current source
    /// </param>
    /// <param name="offset">
    ///   The zero-based byte offset in <paramref name="buffer" /> at which to begin storing the data read
    ///   from the current stream
    /// </param>
    /// <param name="count">The maximum number of bytes to be read from the current stream</param>
    /// <returns>
    ///   The total number of bytes read into the buffer. This can be less than the number of bytes requested if that
    ///   many bytes are not currently available, or zero (0) if the end of the stream has been reached
    /// </returns>
    /// <exception cref="T:System.ArgumentException">
    ///   The sum of <paramref name="offset" /> and <paramref name="count" /> is
    ///   larger than the buffer length
    /// </exception>
    /// <exception cref="T:System.ArgumentNullException">
    ///   <paramref name="buffer" /> is null
    /// </exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    ///   <paramref name="offset" /> or <paramref name="count" /> is negative
    /// </exception>
    /// <exception cref="T:System.IO.IOException">An I/O error occurs</exception>
    /// <exception cref="T:System.NotSupportedException">The stream does not support reading</exception>
    /// <exception cref="T:System.ObjectDisposedException">Methods were called after the stream was closed</exception>
    public override int Read(byte[] buffer, int offset, int count) =>
      this.streams.First(s => s.CanRead).Read(buffer, offset, count);

    /// <inheritdoc />
    /// <summary>
    ///   When overridden in a derived class, writes a sequence of bytes to the current stream and advances the current
    ///   position within this stream by the number of bytes written
    /// </summary>
    /// <param name="buffer">
    ///   An array of bytes. This method copies <paramref name="count" /> bytes from
    ///   <paramref name="buffer" /> to the current stream
    /// </param>
    /// <param name="offset">
    ///   The zero-based byte offset in <paramref name="buffer" /> at which to begin copying bytes to the
    ///   current stream
    /// </param>
    /// <param name="count">The number of bytes to be written to the current stream</param>
    /// <exception cref="T:System.ArgumentException">
    ///   The sum of <paramref name="offset" /> and <paramref name="count" /> is
    ///   greater than the buffer length
    /// </exception>
    /// <exception cref="T:System.ArgumentNullException">
    ///   <paramref name="buffer" />  is null
    /// </exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    ///   <paramref name="offset" /> or <paramref name="count" /> is negative
    /// </exception>
    /// <exception cref="T:System.IO.IOException">An I/O error occured, such as the specified file cannot be found</exception>
    /// <exception cref="T:System.NotSupportedException">The stream does not support writing</exception>
    /// <exception cref="T:System.ObjectDisposedException">
    ///   <see cref="M:System.IO.Stream.Write(System.Byte[],System.Int32,System.Int32)" /> was called after the stream was
    ///   closed
    /// </exception>
    public override void Write(byte[] buffer, int offset, int count) => this.streams.ForEach(s => {
      if (s.CanWrite) { s.Write(buffer, offset, count); }
    });

    #endregion

    #region IEnumerator methods

    /// <inheritdoc />
    /// <summary>Returns an enumerator that iterates through the collection</summary>
    /// <returns>An enumerator that can be used to iterate through the collection</returns>
    public IEnumerator<Stream> GetEnumerator() => this.streams.GetEnumerator();

    /// <inheritdoc />
    /// <summary>Returns an enumerator that iterates through a collection</summary>
    /// <returns>An <see cref="T:System.Collections.IEnumerator" /> object that can be used to iterate through the collection</returns>
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    #endregion

    #region ICollection<T> properties

    /// <inheritdoc />
    /// <summary>
    ///   Gets the number of elements contained in the <see cref="T:System.Collections.Generic.ICollection`1" />
    /// </summary>
    /// <returns>The number of elements contained in the <see cref="T:System.Collections.Generic.ICollection`1" /></returns>
    public int Count => this.streams.Count;

    /// <inheritdoc />
    /// <summary>Gets a value indicating whether the <see cref="T:System.Collections.Generic.ICollection`1" /> is read-only</summary>
    /// <returns>true if the <see cref="T:System.Collections.Generic.ICollection`1" /> is read-only; otherwise, false</returns>
    public bool IsReadOnly => false;

    #endregion

    #region ICollection<T> methods

    /// <inheritdoc />
    /// <summary>Adds an item to the <see cref="T:System.Collections.Generic.ICollection`1" /></summary>
    /// <param name="item">The object to add to the <see cref="T:System.Collections.Generic.ICollection`1" /></param>
    /// <exception cref="T:System.NotSupportedException">
    ///   The <see cref="T:System.Collections.Generic.ICollection`1" /> is
    ///   read-only
    /// </exception>
    public void Add(Stream item) => this.streams.Add(item);

    /// <inheritdoc />
    /// <summary>Removes all items from the <see cref="T:System.Collections.Generic.ICollection`1" /></summary>
    /// <exception cref="T:System.NotSupportedException">
    ///   The <see cref="T:System.Collections.Generic.ICollection`1" /> is
    ///   read-only
    /// </exception>
    public void Clear() => this.streams.Clear();

    /// <inheritdoc />
    /// <summary>Determines whether the <see cref="T:System.Collections.Generic.ICollection`1" /> contains a specific value</summary>
    /// <param name="item">The object to locate in the <see cref="T:System.Collections.Generic.ICollection`1" /></param>
    /// <returns>
    ///   true if <paramref name="item" /> is found in the <see cref="T:System.Collections.Generic.ICollection`1" />;
    ///   otherwise, false
    /// </returns>
    public bool Contains(Stream item) => this.streams.Contains(item);

    /// <inheritdoc />
    /// <summary>
    ///   Copies the elements of the <see cref="T:System.Collections.Generic.ICollection`1" /> to an
    ///   <see cref="T:System.Array" />, starting at a particular <see cref="T:System.Array" /> index
    /// </summary>
    /// <param name="array">
    ///   The one-dimensional <see cref="T:System.Array" /> that is the destination of the elements copied
    ///   from <see cref="T:System.Collections.Generic.ICollection`1" />. The <see cref="T:System.Array" /> must have
    ///   zero-based indexing
    /// </param>
    /// <param name="arrayIndex">The zero-based index in <paramref name="array" /> at which copying begins</param>
    /// <exception cref="T:System.ArgumentNullException">
    ///   <paramref name="array" /> is null
    /// </exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    ///   <paramref name="arrayIndex" /> is less than 0
    /// </exception>
    /// <exception cref="T:System.ArgumentException">
    ///   The number of elements in the source
    ///   <see cref="T:System.Collections.Generic.ICollection`1" /> is greater than the available space from
    ///   <paramref name="arrayIndex" /> to the end of the destination <paramref name="array" />
    /// </exception>
    public void CopyTo(Stream[] array, int arrayIndex) => this.streams.CopyTo(array, arrayIndex);

    /// <inheritdoc />
    /// <summary>
    ///   Removes the first occurrence of a specific object from the
    ///   <see cref="T:System.Collections.Generic.ICollection`1" />
    /// </summary>
    /// <param name="item">The object to remove from the <see cref="T:System.Collections.Generic.ICollection`1" /></param>
    /// <returns>
    ///   true if <paramref name="item" /> was successfully removed from the
    ///   <see cref="T:System.Collections.Generic.ICollection`1" />; otherwise, false. This method also returns false if
    ///   <paramref name="item" /> is not found in the original <see cref="T:System.Collections.Generic.ICollection`1" />
    /// </returns>
    /// <exception cref="T:System.NotSupportedException">
    ///   The <see cref="T:System.Collections.Generic.ICollection`1" /> is
    ///   read-only
    /// </exception>
    public bool Remove(Stream item) => this.streams.Remove(item);

    #endregion

    #region IList<T> methods

    /// <inheritdoc />
    /// <summary>Determines the index of a specific item in the <see cref="T:System.Collections.Generic.IList`1" /></summary>
    /// <param name="item">The object to locate in the <see cref="T:System.Collections.Generic.IList`1" /></param>
    /// <returns>The index of <paramref name="item" /> if found in the list; otherwise, -1</returns>
    public int IndexOf(Stream item) => this.streams.IndexOf(item);

    /// <inheritdoc />
    /// <summary>Inserts an item to the <see cref="T:System.Collections.Generic.IList`1" /> at the specified index</summary>
    /// <param name="index">The zero-based index at which <paramref name="item" /> should be inserted</param>
    /// <param name="item">The object to insert into the <see cref="T:System.Collections.Generic.IList`1" /></param>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    ///   <paramref name="index" /> is not a valid index in the <see cref="T:System.Collections.Generic.IList`1" />
    /// </exception>
    /// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.IList`1" /> is read-only</exception>
    public void Insert(int index, Stream item) => this.streams.Insert(index, item);

    /// <inheritdoc />
    /// <summary>Removes the <see cref="T:System.Collections.Generic.IList`1" /> item at the specified index</summary>
    /// <param name="index">The zero-based index of the item to remove</param>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    ///   <paramref name="index" /> is not a valid index in the <see cref="T:System.Collections.Generic.IList`1" />
    /// </exception>
    /// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.IList`1" /> is read-only</exception>
    public void RemoveAt(int index) => this.streams.RemoveAt(index);

    #endregion
  }
}